using System.Data;
using System.Data.SqlClient;

namespace ItsMY_M;

public class DatabaseConnectionManager
{
    private SqlConnection? _connection;
    private readonly string _connectionString;

    public DatabaseConnectionManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection GetConnection()
    {
        try
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }
            else if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }
        catch (InvalidOperationException ex)
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
            
            return _connection;
        }

        return _connection;
    }

    public void CloseConnection()
    {
        if (_connection != null && _connection.State != ConnectionState.Closed) _connection.Close();
    }

    public bool ChangeUsername(string username, string new_username)
    {
        var query = "UPDATE Users SET Username = @NewUsername WHERE Username = @Username";
        var connection = GetConnection();
        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@NewUsername", new_username);
        command.Parameters.AddWithValue("@Username", username);
        var rowsAffected = command.ExecuteNonQuery();
        if (rowsAffected == 0)
        {
            Console.WriteLine("Failed to update username in database");
            return false;
        }

        CloseConnection();

        return true;
    }

    public bool ChangePassword(string username, string new_password)
    {
        var query = "UPDATE Users SET Password = @NewPassword WHERE Username = @Username";
        var connection = GetConnection();
        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@NewPassword", Utils.StringToSHA256(new_password));
        command.Parameters.AddWithValue("@Username", username);
        var rowsAffected = command.ExecuteNonQuery();
        if (rowsAffected == 0)
        {
            Console.WriteLine("Failed to update username in database");
            return false;
        }

        CloseConnection();

        return true;
    }

    public bool AddMessage(string username, string message, int chatId)
    {
        return AddMessageWithID(GetUserId(username), message, chatId);
    }

    private bool AddMessageWithID(int username, string message, int chatId)
    {
        //  throw new NotImplementedException();
        var query = "INSERT INTO Messages (UserId, ChatId, Msg, Timestamp)" +
                    "VALUES (@UserId, @ChatId, @Msg, @TIMESTAMP);";
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserId", username);
            command.Parameters.AddWithValue("@ChatId", chatId);
            command.Parameters.AddWithValue("@Msg", message);
            command.Parameters.AddWithValue("@TIMESTAMP", (int)(DateTime.UtcNow - Constants.epoch).TotalSeconds);
            connection.Open(); // Open the connection

            var rowsAffected = command.ExecuteNonQuery();
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
        var query = "SELECT u1.Username, u2.Username, c.Guid, c.Id " +
                    "FROM Chats c " +
                    "JOIN Users u1 ON c.Sender = u1.Id " +
                    "JOIN Users u2 ON c.Target = u2.Id " +
                    "WHERE (c.Guid = @Guid)";
        var connection = GetConnection();
        var command = new SqlCommand(query, connection);
        command.Parameters.Add("@Guid", SqlDbType.NVarChar).Value = chat.ToString();
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            toEdit = reader.GetString(0).Equals(username) ? "SenderVis" : "TargetVis";
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
        var command2 = new SqlCommand(query, connection);
        command2.Parameters.AddWithValue("@Id", id);
        Utils.Debug(command2.CommandText);

        var rowsAffected = command2.ExecuteNonQuery();
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
        var username = lm.username;

        var chats = new List<Chat>();
        var query = "SELECT c.Subject, u1.Username, u2.Username, c.Guid, c.Id " +
                    "FROM Chats c " +
                    "JOIN Users u1 ON c.Sender = u1.Id " +
                    "JOIN Users u2 ON c.Target = u2.Id " +
                    "WHERE ((u1.Username = @Username AND c.SenderVis = 1) OR (u2.Username = @Username AND c.TargetVis = 1))";
        var connection = GetConnection();
        var command = new SqlCommand(query, connection);
        command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var chat = new Chat(
                reader.GetString(0),
                reader.GetString(1).Equals(username) ? reader.GetString(2) : reader.GetString(1),
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
        var messages = new List<Message>();
        var query = "SELECT m.Id, m.Msg, m.Timestamp, u.Username FROM Messages m " +
                    "JOIN Users u ON m.UserId = u.Id " +
                    "WHERE ChatId = @ChatId";
        var connection = GetConnection();
        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@ChatId", chatId);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var message = new Message(reader.GetString(3), reader.GetString(1), reader.GetInt64(2));
            messages.Add(message);
        }

        reader.Close();
        CloseConnection();
        return messages;
    }

    public int GetUserId(string username)
    {
        var userId = -1;
        var query = "SELECT Id FROM Users WHERE Username = @Username";
        var connection = GetConnection();
        var command = new SqlCommand(query, connection);

        command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;
        using (var reader = command.ExecuteReader())
        {
            if (reader.Read()) userId = reader.GetInt32(0);
        }

        return userId;
    }

    public int GetChatId(Guid chatGuid)
    {
        var chatId = -1;
        var query = "SELECT Id FROM Chats WHERE Guid = @ChatGuid";
        using (var connection = GetConnection())
        {
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@ChatGuid", SqlDbType.VarChar).Value = chatGuid.ToString();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) chatId = reader.GetInt32(0);
                }
            }
        }

        return chatId;
    }


    public User? GetUserByUsername(string username)
    {
        var query = "SELECT * FROM Users WHERE Username = @Username";
        var connection = GetConnection();
        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Username", username);
        var reader = command.ExecuteReader();
        if (reader.Read())
        {
            var user = new User(
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3)
            );
            reader.Close();
            CloseConnection();
            return user;
        }

        reader.Close();
        CloseConnection();
        return null;
    }

    public string? GetNameByGuid(Guid? guid)
    {
        if (guid == null) return null;

        var query = "SELECT * FROM Users WHERE Guid = @Guid";
        var connection = GetConnection();
        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Guid", guid.ToString());
        var reader = command.ExecuteReader();
        if (reader.Read())
        {
            var tmp = reader.GetString(1);
            reader.Close();
            CloseConnection();
            return tmp;
        }

        reader.Close();
        CloseConnection();
        return null;
    }

    public bool UsernameExists(string username)
    {
        var query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
        var connection = GetConnection();
        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Username", username);
        var count = (int)command.ExecuteScalar();
        CloseConnection();
        return count > 0;
    }

    public bool NewChat(string sender, string target, string subject, string message)
    {
        var guid = Guid.NewGuid();
        var senderId = GetUserId(sender);

        var query = "INSERT INTO Chats (Subject, Sender, Target, Guid, SenderVis, TargetVis)" +
                    "VALUES (@Subject, @Sender, @Target, @Guid, 1, 1);";
        var connection = GetConnection();
        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Subject", subject);
        command.Parameters.AddWithValue("@Target", GetUserId(target));
        command.Parameters.AddWithValue("@Guid", guid.ToString());
        command.Parameters.AddWithValue("@Sender", senderId);
        var rowsAffected = command.ExecuteNonQuery();
        if (rowsAffected == 0)
        {
            Console.WriteLine("Failed to update username in database");
            return false;
        }

        CloseConnection();

        AddMessageWithID(senderId, message, GetChatId(guid));

        return true;
    }
    
    public bool AddUser(string username, string password)
    {
        var guid = Guid.NewGuid();
        var query = "INSERT INTO Users (Username, Password, Guid) " +
                    "VALUES (@Username, @Password, @Guid);";
        var connection = GetConnection();
        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Username", username);
        command.Parameters.AddWithValue("@Password", Utils.StringToSHA256( password));
        command.Parameters.AddWithValue("@Guid", guid.ToString());
        var rowsAffected = command.ExecuteNonQuery();
        if (rowsAffected == 0)
        {
            Console.WriteLine("Failed to add user to database");
            return false;
        }
        CloseConnection();
        return true;
    }

}
