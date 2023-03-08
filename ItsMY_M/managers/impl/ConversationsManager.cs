using ItsMY_M.commands;

namespace ItsMY_M.managers;

public class ConversationsManager : AManager
{
    public ConversationsManager(AManager manager) : base(manager.db, manager.lm)
    {
        AddCommand(new SelectChat(this));
        AddCommand(new RemoveChat(this));
    }
}