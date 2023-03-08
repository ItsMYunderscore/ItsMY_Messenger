using ItsMY_M.managers;

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
        if (!IsPasswordSafe(password))
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

    private bool IsPasswordSafe(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) return false;

        if (password.Length < 8) return false;

        var hasUppercase = false;
        var hasLowercase = false;
        var hasDigit = false;

        foreach (var c in password)
            if (char.IsUpper(c))
                hasUppercase = true;
            else if (char.IsLower(c))
                hasLowercase = true;
            else if (char.IsDigit(c)) hasDigit = true;

        if (!hasUppercase || !hasLowercase || !hasDigit) return false;

        return true;
    }
}