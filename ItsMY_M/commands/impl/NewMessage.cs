using ItsMY_M.managers;

namespace ItsMY_M.commands;

public class NewMessage : ACommand
{
    public NewMessage(AManager manager) : base(manager, "New Message")
    {
    }


    public override void Do()
    {
        var target = Utils.GetStringArgument("Target user (username)");
        var subject = Utils.GetStringArgument("Subject");
        var message = Utils.GetStringArgument("Message");

        if (!db.UsernameExists(target))
        {
            Console.WriteLine("No user with that name found...\n");
            return;
        }

        db.NewChat(lm.username, target, subject, message);
        Console.WriteLine("Message sent!");

        //new ChatManager(_manager, null).StartSelection();
    }
}