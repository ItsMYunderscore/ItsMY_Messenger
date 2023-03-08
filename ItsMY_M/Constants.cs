namespace ItsMY_M;

public class Constants
{
    public const string con_str = "Server=localhost\\SQLEXPRESS;Database=Messenger;Trusted_Connection=True;";
    public const bool debug = true;
    public static DatabaseConnectionManager DatabaseConnectionManager = new(con_str);
    public static readonly DateTime epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
}