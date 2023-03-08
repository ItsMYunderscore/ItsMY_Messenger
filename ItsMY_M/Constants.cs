namespace ItsMY_M;

public class Constants
{
    public static DatabaseConnectionManager DatabaseConnectionManager = new DatabaseConnectionManager(con_str);
    
    public const string con_str = "Server=localhost\\SQLEXPRESS;Database=Messenger;Trusted_Connection=True;";
    public static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public const bool debug = true;
}