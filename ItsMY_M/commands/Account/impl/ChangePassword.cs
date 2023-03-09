using ItsMY_M.managers;
using ItsMY_M.Utils;

namespace ItsMY_M.commands;

public class ChangePassword : AAccountACommand
{
    public ChangePassword(AManager manager) : base(manager, "Change Password")
    {
    }

    public override void Logic()
    {
        var currentPassword = Utils.GetStringArgument("Current password");
        var newPassword = Utils.GetStringArgument("New password");
        var newPasswordAgain = Utils.GetStringArgument("New password again");

        if (!lm.ComparePasswords(currentPassword, lm.User) && !newPassword.Equals(newPasswordAgain))
        {
            Console.WriteLine("Something went wrong... try again");
            return;
        }

        ChangePasswd(newPassword);
    }

    private void ChangePasswd(string password)
    {
        if (!Utils.IsPasswordSafe(password))
        {
            Console.WriteLine(
                "Invalid password!\nThe password should be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one digit.");
            return;
        }

        if (lm.username == null)
        {
            Console.WriteLine("Exception 1x01");
            return;
        }

        db.ChangePassword(lm.username, password);
        Console.WriteLine("Password changed");
    } 
}
