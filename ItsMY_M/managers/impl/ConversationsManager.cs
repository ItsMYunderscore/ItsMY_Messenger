using System.Text;
using ItsMY_M.commands;

namespace ItsMY_M.managers;

public class ConversationsManager : AManager
{
    public ConversationsManager(AManager manager) : base(manager.db, manager.lm)
    {
        AddCommand(new SelectChat(this));
        AddCommand(new RemoveChat(this));
        AddCommand(new NewMessage(this));
    }

    public override void DoInLoop()
    {
        var chats = db.GetChatsByUser(lm);

        var stringBuilder = new StringBuilder("Chats: \n");

        for (var i = 0; i < chats.Count; i++)
            stringBuilder = stringBuilder.Append(i + 1 + " | " + chats[i].Target + " | " + chats[i].Subject + "\n");

        Console.WriteLine(stringBuilder.ToString());
    }
}