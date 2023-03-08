using ItsMY_M.managers;

namespace ItsMY_M.commands;

public class Register : ACommand
{
    public Register(AManager manager) : base(manager, "Registration")
    {
    }
}