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
        var chats = db.GetChatsByUser(lm);

        var stringBuilder = new StringBuilder("Chats: \n");

        for (var i = 0; i < chats.Count; i++)
            stringBuilder = stringBuilder.Append(i + 1 + " | " + chats[i].Target + " | " + chats[i].Subject + "\n");

        Console.WriteLine(stringBuilder.ToString());

        var resp = Utils.GetResponse("Action", chats.Count, true);

        if (resp == 0) return;

        db.RemoveChat(lm.username, chats[resp - 1].Guid);
    }
}