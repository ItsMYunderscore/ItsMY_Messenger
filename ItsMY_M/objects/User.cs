namespace ItsMY_M;

public class User
{
    public Guid Guid;
    public string Password;
    public string Username;

    public User(string username, string password, string guid)
    {
        Username = username;
        Password = password;
        Guid = Guid.Parse(guid);
    }
}