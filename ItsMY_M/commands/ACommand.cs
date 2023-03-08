using ItsMY_M.managers;

namespace ItsMY_M.commands;

public class ACommand
{
    protected readonly AManager _manager;
    protected readonly DatabaseConnectionManager db;
    public readonly string Description;
    protected readonly LoginManager lm;

    public ACommand(AManager manager, string description)
    {
        _manager = manager;
        db = manager.db;
        lm = manager.lm;
        Description = description;
    }

    public virtual void Do()
    {
        throw new NotImplementedException();
    }

    public void Call()
    {
        Utils.Debug("Called " + Description);
        Do();
    }
}