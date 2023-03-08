using System.Text;
using ItsMY_M.managers;

namespace ItsMY_M.commands;

public class RemoveChat : ACommand
{
    public RemoveChat(AManager manager) : base(manager, "Remove chat")
    {
    }

    public override void Do()
    {
        List<Chat> chats = db.GetChatsByUser(lm);

        StringBuilder stringBuilder = new StringBuilder("Chats: \n");
        
        for (var i = 0; i < chats.Count; i++)
        {
            stringBuilder = stringBuilder.Append((i + 1) + " | " + chats[i].Target + " | " + chats[i].Subject + "\n");
        }

        Console.WriteLine(stringBuilder.ToString());
        
        int resp = Utils.GetResponse("Action", chats.Count, true);

        if (resp == 0) return;

        db.RemoveChat(lm.username, chats[resp].Guid);
    }
}