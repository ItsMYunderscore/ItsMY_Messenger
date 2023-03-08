using ItsMY_M.commands;
using ItsMY_M.managers;

namespace ItsMY_M;

public class AccountManager : AManager
{

    public AccountManager(AManager manager) : base(manager.db, manager.lm)
    {
        AddCommand(new Login(this));
        AddCommand(new Register(this));
        AddCommand(new Logout(this));
        AddCommand(new ChangePassword(this));
        AddCommand(new ChangeNickname(this));
    }
    
}