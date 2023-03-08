using static ItsMY_M.Utils;

namespace ItsMY_M;

public class LoginManager
{
    private DatabaseConnectionManager _databaseManager;
    private User? _user;

    public User? User
    {
        set
        {
            _user = value;
        }
        get
        {
            return _user;
        }
    }

    public string? username
    {
        get
        {
            Guid? guid = null;

            if (_user != null)
            {
                guid = _user.Guid;
            }
            
            string? name = _databaseManager.GetNameByGuid(guid);

            if (name != null && _user != null)
            {
                _user.Username = name;
            }

            return name;
        } 
    }

    public LoginManager(DatabaseConnectionManager databaseManager)
    {
        this._databaseManager = databaseManager;
    }
    
    public void ForceLogin(string name)
    {
        User tmp = _databaseManager.GetUserByUsername(name);

        _user = tmp;
    }

    public void PreformLogin()
    {
        int i = 0;
        
        while (i <= 3)
        {
            Console.WriteLine("Username: ");
            string? response = Console.ReadLine();

            if (response == null || response.Contains(' ')) continue;

            User? tmp = _databaseManager.GetUserByUsername(response);

            if (PreformPasswordCheck(tmp))
            {
                _user = tmp;
                return;
            }

            i++;
        }
    }

    private bool PreformPasswordCheck(User? user)
    {
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine("Password: ");
            
            string? response = Console.ReadLine();
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
