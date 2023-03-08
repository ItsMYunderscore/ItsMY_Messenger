using static ItsMY_M.Utils;

namespace ItsMY_M;

public class LoginManager
{
    private readonly DatabaseConnectionManager _databaseManager;

    public LoginManager(DatabaseConnectionManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    public User? User { set; get; }

    public string? username
    {
        get
        {
            Guid? guid = null;

            if (User != null) guid = User.Guid;

            var name = _databaseManager.GetNameByGuid(guid);

            if (name != null && User != null) User.Username = name;

            return name;
        }
    }

    public void ForceLogin(string name)
    {
        var tmp = _databaseManager.GetUserByUsername(name);

        User = tmp;
    }

    public void PreformLogin()
    {
        var i = 0;

        while (i <= 3)
        {
            Console.WriteLine("Username: ");
            var response = Console.ReadLine();

            if (response == null || response.Contains(' ')) continue;

            var tmp = _databaseManager.GetUserByUsername(response);

            if (PreformPasswordCheck(tmp))
            {
                User = tmp;
                return;
            }

            i++;
        }
    }

    private bool PreformPasswordCheck(User? user)
    {
        for (var i = 0; i < 3; i++)
        {
            Console.WriteLine("Password: ");

            var response = Console.ReadLine();
            if (response == null) continue;

            if (ComparePasswords(response, user))
            {
                Console.WriteLine("Login successful!\n\n");
                return true;
            }

            Console.WriteLine("Wrong password!\n");
            Thread.Sleep(new Random(700).Next(2000));
        }

        return false;
    }

    public bool ComparePasswords(string passwd, User? user)
    {
        if (user == null) return false;

        return user.Password.Equals(StringToSHA256(passwd));
    }
}