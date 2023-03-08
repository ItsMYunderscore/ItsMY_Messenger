using System.Text;

namespace ItsMY_M;

public class Chat
{
    public readonly int _id;
    private readonly DatabaseConnectionManager db;
    public readonly Guid Guid;
    private readonly LoginManager lm;
    public List<Message> Messages = new();
    public string Subject;
    public string Target;

    public Chat(string subject, string target, string guid, int id, DatabaseConnectionManager db, LoginManager lm)
    {
        Subject = subject;
        Target = target;
        Guid = Guid.Parse(guid);
        _id = id;
        this.db = db;
        this.lm = lm;
    }

    public string List => Target + " | " + Subject;

    public string ListAllMessages
    {
        get
        {
            var sb = new StringBuilder(List + " | " + Guid + "\n");

            LoadMessages();

            foreach (var message in Messages) sb = sb.Append(message.Msg + "\n");

            return sb.ToString();
        }
    }

    public void LoadMessages()
    {
        Messages = db.GetMessagesForChat(_id);
    }
}