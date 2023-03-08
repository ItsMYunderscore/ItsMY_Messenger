using ItsMY_M.managers;

namespace ItsMY_M.commands;

public class Respond : ACommand
{
    private readonly Chat chat;

    public Respond(AManager manager, Chat chat) : base(manager, "Respond")
    {
        this.chat = chat;
    }

    public override void Do()
    {
        Console.WriteLine(chat.ListAllMessages);

        var o = Utils.GetMessage();

        if (o == null)
        {
            Console.WriteLine("Message can't be empty!");
            return;
        }

        if (o.Length > 500)
        {
            Console.WriteLine("\nToo long message!\n");
            return;
        }

        if (!Utils.AddMessage(lm.username, o, chat._id))
        {
            Console.WriteLine("\nFailed to send message!\n");
            return;
        }

        chat.LoadMessages();
    }
}