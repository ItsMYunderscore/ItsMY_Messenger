using ItsMY_M.managers;

namespace ItsMY_M.commands;

public class NewMessage : ACommand
{
    public NewMessage(AManager manager) : base(manager, "New Message")
    {
    }


    public override void Do()
    {
        throw new NotImplementedException();
    }
}