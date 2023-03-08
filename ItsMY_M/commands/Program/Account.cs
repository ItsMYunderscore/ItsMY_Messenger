using ItsMY_M.managers;

namespace ItsMY_M.commands.Program;

public class Account : ACommand
{
    public Account(AManager manager) : base(manager, "Open Account Settings")
    {
    }

    public override void Do()
    {
        new AccountManager(_manager).StartSelection();
    }
}