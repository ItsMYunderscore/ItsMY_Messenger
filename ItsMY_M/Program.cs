// See https://aka.ms/new-console-template for more information

using ItsMY_M;
using ItsMY_M.managers;

Console.WriteLine("Hello, World!");

DatabaseConnectionManager db = new DatabaseConnectionManager(Constants.con_str);

LoginManager lm = new LoginManager(db);
//lm.PreformLogin();
//lm.ForceLogin("root");

//Console.WriteLine(Utils.StringToSHA256(""));

//List<Chat> chats = db.GetChatsByUser( lm);

/*foreach (Chat chat in chats)
{
    db.RemoveChat(lm.username, chat.Guid);
}*/
new ProgramManager(db, lm).StartSelection();
//db.RemoveChat("admin", new Guid());
//db.AddMessage("admin", "test", 1);