using ItsMY_M.managers;

namespace ItsMY_M.commands;

public class Register : ACommand
{
    public Register(AManager manager) : base(manager, "Registration")
    {
    }

    public override void Do()
    {
        if (lm.username != null)
        {
            Console.WriteLine("You are already logged in...\n");
            return;
        }

        string username = Utils.GetStringArgument("Username");
        string newPassword = Utils.GetStringArgument("Password");
        string newPasswordAgain = Utils.GetStringArgument("Password again");

        if (!newPassword.Equals(newPasswordAgain) || !db.AddUser(username, newPasswordAgain))
        {
            Console.WriteLine("Something went wrong... try again");
            return;
        }

        lm.User = db.GetUserByUsername(username);
        Console.WriteLine("Registered and logged in as " + username + "\n");
    }
}