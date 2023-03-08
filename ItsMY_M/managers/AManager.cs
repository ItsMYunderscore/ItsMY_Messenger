using System.Text;
using ItsMY_M.commands;

namespace ItsMY_M.managers;

public class AManager
{
    public readonly LoginManager lm;
    protected List<ACommand> Commands = new();
    public DatabaseConnectionManager db;

    public AManager(DatabaseConnectionManager db, LoginManager lm)
    {
        this.db = db;
        this.lm = lm;
    }

    private string com
    {
        get
        {
            var sb = new StringBuilder("0 - Back | ");

            for (var i = 0; i < Commands.Count; i++) sb.Append(i + 1 + " - " + Commands[i].Description + " | ");

            return sb.ToString().Substring(0, sb.ToString().Length - 3);
        }
    }

    public void StartSelection()
    {
        if (Commands.Count == 0) return;

        while (true)
        {
            DoInLoop();
            Console.WriteLine(com);
            var resp = Utils.GetResponse("Action", Commands.Count, true);

            if (resp == 0) return;

            Utils.Debug(resp + "");

            Commands[resp - 1].Call();
        }
    }

    public virtual void DoInLoop()
    {
    }

    protected void AddCommand(ACommand aCommand)
    {
        Commands.Add(aCommand);
    }
}