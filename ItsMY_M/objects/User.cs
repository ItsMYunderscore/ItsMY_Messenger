namespace ItsMY_M;

public class User
{
    public string Username;
    public string Password;
    public Guid Guid;

    public User(string username, string password, string guid)
    {
        Username = username;
        Password = password;
        Guid = Guid.Parse(guid);
    }
}