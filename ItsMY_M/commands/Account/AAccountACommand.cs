using ItsMY_M.managers;

namespace ItsMY_M.commands;

public class AAccountACommand : ACommand
{
    public AAccountACommand(AManager manager, string description) : base(manager, description)
    {
    }

    public virtual void Logic()
    {
        throw new NotImplementedException();
    }

    public override void Do()
    {
        if (lm.username == null)
        {
            Console.WriteLine("You need to be logged in for this!");
            Utils.Debug("lm.username == null");
            return;
        }

        Utils.Debug("lm.username == " + lm.username);

        Logic();
    }
}