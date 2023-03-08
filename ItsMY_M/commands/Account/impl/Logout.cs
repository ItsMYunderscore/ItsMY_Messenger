using ItsMY_M.managers;

namespace ItsMY_M.commands;

public class Logout : AAccountACommand
{
    public Logout(AManager manager) : base(manager, "Logout")
    {
    }

    public override void Logic()
    {
        lm.User = null;

        Console.WriteLine("Logged out...\n");
    }
}