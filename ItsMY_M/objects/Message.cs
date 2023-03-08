namespace ItsMY_M;

public class Message
{
    public string msg;
    public long Timestamp;
    public string User;

    public Message(string user, string message, long timestamp)
    {
        User = user;
        Msg = message;
        Timestamp = timestamp;
    }

    public string Msg
    {
        get
        {
            var dat_Time = Constants.epoch;
            dat_Time = dat_Time.AddSeconds(Timestamp);
            return User + " [" + dat_Time.ToString("dd/MM/yyyy H:mm") + "]\n" + msg;
        }
        set => msg = value;
    }
}