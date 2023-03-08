using ItsMY_M.commands.Program;
using ItsMY_M.managers;

namespace ItsMY_M;

public class ProgramManager : AManager
{
    public ProgramManager(DatabaseConnectionManager db, LoginManager lm) : base(db, lm)
    {
        AddCommand(new Account(this));
        AddCommand(new commands.Program.Chat(this));
    }
}