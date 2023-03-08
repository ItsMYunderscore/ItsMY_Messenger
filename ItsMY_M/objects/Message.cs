using System.Reflection.Metadata.Ecma335;

namespace ItsMY_M
{
    public class Message
    {
        public string User;
        public long Timestamp;
        public string msg;
        public string Msg
        {
            get
            {
                System.DateTime dat_Time = Constants.epoch;
                dat_Time = dat_Time.AddSeconds(this.Timestamp);
                return this.User + " [" + dat_Time.ToString("dd/MM/yyyy H:mm") + "]\n" + this.msg;
            }
            set
            {
                msg = value;
            }
        }
        public Message(string user, string message, long timestamp)
        {
            this.User = user;
            this.Msg = message;
            this.Timestamp = timestamp;
        }
    }
}