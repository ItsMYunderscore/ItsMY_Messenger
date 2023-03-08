using ItsMY_M.managers;

namespace ItsMY_M.commands;

public class Login : ACommand
{
    public Login(AManager manager) : base(manager, "Login")
    {
    }

    public override void Do()
    {
        if (lm.username != null)
        {
            Console.WriteLine("You are already logged in...\n");
            return;
        }

        PreformLogin();
    }

    public void PreformLogin()
    {
        var i = 0;

        while (i <= 3)
        {
            Console.WriteLine("Username: ");
            var response = Console.ReadLine();

            if (response == null || response.Contains(' ')) continue;

            var tmp = db.GetUserByUsername(response);

            if (PreformPasswordCheck(tmp))
            {
                lm.User = tmp;
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
        }

        return false;
    }

    private bool ComparePasswords(string passwd, User? user)
    {
        if (user == null) return false;

        return user.Password.Equals(Utils.StringToSHA256(passwd));
    }
}