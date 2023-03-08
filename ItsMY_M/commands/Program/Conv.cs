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
        List<ItsMY_M.Chat> chats = db.GetChatsByUser(lm);

        if (chats.Count == 0)
        {
            Console.WriteLine("There are no chats");    
            return;
        }

        StringBuilder stringBuilder = new StringBuilder("Messages: \n");
        
        for (int i = 0; i < chats.Count; i++)
        {
            stringBuilder = stringBuilder.Append((i + 1) + " | " +chats[i].List);
        }
        
        Console.WriteLine(stringBuilder.ToString());

        int resp = Utils.GetResponse("Chat id", chats.Count, false)-1;

        new ChatManager(_manager, chats[resp]).StartSelection();

    }
}