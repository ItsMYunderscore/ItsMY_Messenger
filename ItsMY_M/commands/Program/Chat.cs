using ItsMY_M.managers;

namespace ItsMY_M.commands.Program;

public class Chat : AAccountACommand
{
    public Chat(AManager manager) : base(manager, "Open chats")
    {
    }

    public override void Logic()
    {
        new ConversationsManager(_manager).StartSelection();
    }
}