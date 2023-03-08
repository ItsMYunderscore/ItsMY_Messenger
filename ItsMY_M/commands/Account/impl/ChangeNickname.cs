using ItsMY_M.managers;

namespace ItsMY_M.commands;

public class ChangeNickname : AAccountACommand
{
    public ChangeNickname(AManager manager) : base(manager, "Change nickname")
    {
    }

    public override void Logic()
    {
        if (db.ChangeUsername(lm.username, Utils.GetStringArgument("New username")))
        {
            Console.WriteLine("Username changed to " + lm.username);
            return;
        }

        Console.WriteLine("Something went wrong... Try again");
    }
}