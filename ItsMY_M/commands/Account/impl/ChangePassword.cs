using ItsMY_M.managers;

namespace ItsMY_M.commands;

public class ChangePassword : AAccountACommand
{
    public ChangePassword(AManager manager) : base(manager, "Change Password")
    {
    }

    public override void Logic()
    {
        string currentPassword = Utils.GetStringArgument("Current password");
        string newPassword = Utils.GetStringArgument("New password");
        string newPasswordAgain = Utils.GetStringArgument("New password again");

        if (!lm.ComparePasswords(currentPassword, lm.User) && !(newPassword.Equals(newPasswordAgain)))
        {
            Console.WriteLine("Something went wrong... try again");
            return;
        }
        
        ChangePasswd(newPassword);
    }

    private void ChangePasswd(string password)
    {
        if (!IsPasswordSafe(password))
        {
            Console.WriteLine("Invalid password!\nThe password should be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one digit.");
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
    
    private bool IsPasswordSafe(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        if (password.Length < 8)
        {
            return false;
        }

        bool hasUppercase = false;
        bool hasLowercase = false;
        bool hasDigit = false;

        foreach (char c in password)
        {
            if (char.IsUpper(c))
            {
                hasUppercase = true;
            }
            else if (char.IsLower(c))
            {
                hasLowercase = true;
            }
            else if (char.IsDigit(c))
            {
                hasDigit = true;
            }
        }

        if (!hasUppercase || !hasLowercase || !hasDigit)
        {
            return false;
        }

        return true;
    }

}