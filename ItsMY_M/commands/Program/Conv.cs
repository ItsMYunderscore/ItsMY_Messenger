using System.Text;
using ItsMY_M.managers;

namespace ItsMY_M.commands.Program;

public class Conv : ACommand
{
    public Conv(AManager manager) : base(manager, "Open Chat")
    {
    }

    public override void Do()
    {
        var chats = db.GetChatsByUser(lm);

        if (chats.Count == 0)
        {
            Console.WriteLine("There are no chats");
            return;
        }

        var stringBuilder = new StringBuilder("Messages: \n");

        for (var i = 0; i < chats.Count; i++) stringBuilder = stringBuilder.Append(i + 1 + " | " + chats[i].List);

        Console.WriteLine(stringBuilder.ToString());

        var resp = Utils.GetResponse("Chat id", chats.Count, false) - 1;

        new ChatManager(_manager, chats[resp]).StartSelection();
    }
}