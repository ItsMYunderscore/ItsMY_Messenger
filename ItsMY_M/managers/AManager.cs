using System.Text;
using ItsMY_M.commands;

namespace ItsMY_M.managers;

public class AManager
{
    public DatabaseConnectionManager db;
    public readonly LoginManager lm;
    protected List<ACommand> Commands = new List<ACommand>();
    private string com
    {
        get
        {
            StringBuilder sb = new StringBuilder("0 - Back | ");
            
            for (int i = 0; i < Commands.Count; i++)
            {
                sb.Append((i+1) + " - " + Commands[i].Description + " | ");
            }

            return sb.ToString().Substring(0,sb.ToString().Length-3);
        }
    }

    public AManager(DatabaseConnectionManager db, LoginManager lm)
    {
        this.db = db;
        this.lm = lm;
    }

    public void StartSelection()
    {
        if (Commands.Count == 0) return;

        while (true)
        {
            DoInLoop();
            Console.WriteLine(com);
            int resp = Utils.GetResponse("Action", Commands.Count, true);

            if (resp == 0) return;
            
            Utils.Debug(resp + "");
            
            Commands[resp-1].Call();
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