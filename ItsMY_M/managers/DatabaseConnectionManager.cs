using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ItsMY_M
{
    public class DatabaseConnectionManager
    {
        private SqlConnection _connection;
        private readonly string _connectionString;

        public DatabaseConnectionManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }
            else if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }

            return _connection;
        }

        public void CloseConnection()
        {
            if (_connection != null && _connection.State != System.Data.ConnectionState.Closed)
            {
                _connection.Close();
            }
        }

        public bool ChangeUsername(string username, string new_username)
        {
            string query = "UPDATE Users SET Username = @NewUsername WHERE Username = @Username";
            SqlConnection connection = GetConnection();
            SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NewUsername", new_username);
                command.Parameters.AddWithValue("@Username", username);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    Console.WriteLine ("Failed to update username in database");
                    return false;
                }
                
                CloseConnection();

                return true;
        }
        
        public bool ChangePassword(string username, string new_password)
        {
            string query = "UPDATE Users SET Password = @NewPassword WHERE Username = @Username";
            SqlConnection connection = GetConnection();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NewPassword", Utils.StringToSHA256( new_password));
            command.Parameters.AddWithValue("@Username", username);
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                Console.WriteLine ("Failed to update username in database");
                return false;
            }
                
            CloseConnection();

            return true;
        }
        
        public bool AddMessage(string username, string message, int chatId)
        {
          //  throw new NotImplementedException();
          string query = "INSERT INTO Messages (UserId, ChatId, Msg, Timestamp)" +
                         "VALUES (@UserId, @ChatId, @Msg, @TIMESTAMP);";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", GetUserId(username));
                command.Parameters.AddWithValue("@ChatId", chatId);
                command.Parameters.AddWithValue("@Msg", message); //
                command.Parameters.AddWithValue("@TIMESTAMP", ((int)(DateTime.UtcNow - Constants.epoch).TotalSeconds));
                connection.Open(); // Open the connection
                
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    Console.WriteLine("Failed to write msg in database");
                    return false;
                }
                // Connection will be closed automatically when exiting the using block
            }
            return true;
        }

        public bool RemoveChat(string username, Guid chat)
        {
            string? toEdit = null;
            int? id = null;
            string query = "SELECT u1.Username, u2.Username, c.Guid, c.Id " +
                           "FROM Chats c " +
                           "JOIN Users u1 ON c.Sender = u1.Id " +
                           "JOIN Users u2 ON c.Target = u2.Id " +
                           "WHERE (c.Guid = @Guid)";
            SqlConnection connection = GetConnection();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@Guid", SqlDbType.NVarChar).Value = chat.ToString();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                toEdit = (reader.GetString(0).Equals(username) ? "SenderVis" : "TargetVis");
                id = reader.GetInt32(3);
            }
            reader.Close();

            if (toEdit == null || id == null)
            {
                Console.WriteLine("Exception 0x01");
                CloseConnection();
                return false;
            }

            query = "UPDATE Chats SET " + toEdit + " = 0 WHERE (Id = @Id)";
            SqlCommand command2 = new SqlCommand(query, connection);
            command2.Parameters.AddWithValue("@Id", id);
            Utils.Debug(command2.CommandText);

            int rowsAffected = command2.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                Console.WriteLine("Failed to update visibility in database");
                CloseConnection();
                return false;
            }
            
            CloseConnection();

            return true;
        }


        
        public List<Chat> GetChatsByUser(LoginManager lm)
        {
            string username = lm.username;
            
            List<Chat> chats = new List<Chat>();
            string query = "SELECT c.Subject, u1.Username, u2.Username, c.Guid, c.Id " +
                           "FROM Chats c " +
                           "JOIN Users u1 ON c.Sender = u1.Id " +
                           "JOIN Users u2 ON c.Target = u2.Id " +
                           "WHERE (u1.Username = @Username OR u2.Username = @Username)";
            SqlConnection connection = GetConnection();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Chat chat = new Chat(
                    reader.GetString(0),
                    (reader.GetString(1).Equals(username) ? reader.GetString(2) : username),
                    reader.GetString(3),
                    reader.GetInt32(4),
                    this,
                    lm
                );
                chats.Add(chat);
            }
            reader.Close();
            CloseConnection();
            return chats;
        }
        
        public List<Message> GetMessagesForChat(int chatId)
        {
            List<Message> messages = new List<Message>();
            string query = "SELECT m.Id, m.Msg, m.Timestamp, u.Username FROM Messages m " +
                           "JOIN Users u ON m.UserId = u.Id " +
                           "WHERE ChatId = @ChatId";
            SqlConnection connection = GetConnection();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ChatId", chatId);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Message message = new Message(reader.GetString(3), reader.GetString(1), reader.GetInt64(2));
                messages.Add(message);
            }
            reader.Close();
            CloseConnection();
            return messages;
        }

        public int GetUserId(string username)
        {
            int userId = -1;
            string query = "SELECT Id FROM Users WHERE Username = @Username";
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = reader.GetInt32(0);
                        }
                    }
                }
            }
            return userId;
        }

        public User? GetUserByUsername(string username)
        {
            string query = "SELECT * FROM Users WHERE Username = @Username";
            SqlConnection connection = GetConnection();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                User user = new User(
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3)
                );
                reader.Close();
                CloseConnection();
                return user;
            }
            else
            {
                reader.Close();
                CloseConnection();
                return null;
            }
        }
        
        public string? GetNameByGuid(Guid? guid)
        {
            if (guid == null) return null;
            
            string query = "SELECT * FROM Users WHERE Guid = @Guid";
            SqlConnection connection = GetConnection();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Guid", guid.ToString());
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                string tmp = reader.GetString(1);
                reader.Close();
                CloseConnection();
                return tmp;
            }
            else
            {
                reader.Close();
                CloseConnection();
                return null;
            }
        }

        public bool UsernameExists(string username)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            SqlConnection connection = GetConnection();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);
            int count = (int)command.ExecuteScalar();
            CloseConnection();
            return count > 0;
        }
    }
}
