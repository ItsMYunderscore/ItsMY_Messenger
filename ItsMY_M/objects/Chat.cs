using System.Text;

namespace ItsMY_M;

public class Chat   
{
    public List<Message> Messages = new List<Message>();
    public string Subject;
    public string Target;
    public readonly Guid Guid;
    public readonly int _id;
    private readonly DatabaseConnectionManager db;
    private readonly LoginManager lm;

    public string List
    {
        get
        {
            return this.Target + " | " + this.Subject;
        }
    }
    
    public string ListAllMessages
    {
        get
        {
            StringBuilder sb = new StringBuilder(List + " | " + Guid.ToString());
            
            LoadMessages();
            
            foreach (Message message in Messages)
            {
                sb = sb.Append(message.Msg + "\n");
            }
            
            return sb.ToString();
        }
    }

    public Chat(string subject, string target, string guid, int id, DatabaseConnectionManager db, LoginManager lm)
    {
        Subject = subject;
        Target = target;
        Guid = System.Guid.Parse(guid);
        _id = id;
        this.db = db;
        this.lm = lm;

    }

    public void LoadMessages()
    {
        Messages = db.GetMessagesForChat(_id);
    }
}