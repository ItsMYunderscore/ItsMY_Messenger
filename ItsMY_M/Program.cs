// See https://aka.ms/new-console-template for more information

using ItsMY_M;

Console.WriteLine("Hello, World!");

DatabaseConnectionManager db = new DatabaseConnectionManager(Constants.con_str);
LoginManager lm = new LoginManager(db);
//lm.ForceLogin("kukelka");

new ProgramManager(db, lm).StartSelection();
