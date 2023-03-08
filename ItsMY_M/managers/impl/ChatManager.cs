using ItsMY_M.commands;
using ItsMY_M.managers;

namespace ItsMY_M;

public class ChatManager : AManager
{
    private Chat chat;
    
    public ChatManager(AManager manager, Chat chat) : base(manager.db, manager.lm)
    {
        this.chat = chat;
        
        AddCommand(new Respond(this, this.chat));
        StartSelection();
    }

    public override void DoInLoop()
    {
        Console.WriteLine(chat.ListAllMessages);
    }
}