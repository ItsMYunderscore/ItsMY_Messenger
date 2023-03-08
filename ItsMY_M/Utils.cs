using System.Security.Cryptography;
using System.Text;

namespace ItsMY_M;

public class Utils
{
    public static string StringToSHA256(string value)
    {
        using var hash = SHA256.Create();
        var byteArray = hash.ComputeHash(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(byteArray);
    }

    public static int GetResponse(string question, int max, bool CanBeZero)
    {
        Console.WriteLine(question + ": ");
        var response = Console.ReadLine();

        int o;

        if (response == null || !int.TryParse(response, out o) || (!CanBeZero && o == 0) || o < 0 || o > max)
        {
            Console.WriteLine("illegal argument... Try again...");
            return GetResponse(question, max, CanBeZero);
        }

        return o;
    }

    public static string? GetMessage()
    {
        Console.WriteLine("Message: ");
        var response = Console.ReadLine();

        if (response == null || !(response.Length > 0))
        {
            Console.WriteLine("illegal argument... Try again...");
            return null;
        }

        return response;
    }

    public static string GetStringArgument(string que)
    {
        Console.WriteLine(que + ": ");
        var response = Console.ReadLine();

        if (response is not { Length: > 0 } || response.Contains(' '))
        {
            Console.WriteLine("illegal argument... Try again...");
            return GetStringArgument(que);
        }

        return response;
    }

    public static bool AddMessage(string username, string message, int chatId)
    {
        return Constants.DatabaseConnectionManager.AddMessage(username, message, chatId);
    }

    public static bool NewChat(string sender, string target, string subject, string message)
    {
        return Constants.DatabaseConnectionManager.NewChat(sender, target, subject, message);
    }

    public static void Debug(string msg)
    {
        if (Constants.debug) Console.WriteLine("[DEBUG] - " + msg);
    }
}