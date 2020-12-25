using System;
using System.Data.SQLite;
using System.IO;

namespace NotesApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //show name app
            ShowNameApp();

            //Lay ten nguoi dung và kiểm tra tồn tại, nếu không tồn tại hỏi người dùng 
            //có muốn tạo mới, không tạo mới thì đánh lại tên người dùng khác
            string userName = GetUserName();

            Console.WriteLine("[1]. Show all notes");
            Console.WriteLine("[2]. Add a new note");
            Console.WriteLine("[3]. Edit note");
            Console.WriteLine("[4]. Delete note");
            Console.Write("Your choice:");
            string choice = Console.ReadLine();
            ShowAllNotes(userName);
            Console.ReadKey();
            //Hỏi người dùng add thêm Note
            static void AddANewNote(){

            }
            string cs = @"URI=file:" + userName + ".db";
            using var con = new SQLiteConnection(cs);
            con.Open();
            using var cmd = new SQLiteCommand(con);
            ////cmd.CommandText = "DROP TABLE IF EXISTS Notes";
            ////cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS  Notes(id INTEGER PRIMARY KEY,
                    date TEXT,title TEXT, content TEXT)";
            cmd.ExecuteNonQuery();
            //cách insert ngày giờ 
            //cmd.CommandText = "INSERT INTO Notes(date, title,content) VALUES(CURRENT_TIMESTAMP, @title,@content)";
            //cách khác:
            cmd.CommandText = "INSERT INTO Notes(date, title,content) VALUES(datetime('now', 'localtime'), @title, @content)";
            //cmd.Parameters.AddWithValue("@date",CURRENT_TIMESTAMP);
            string title = Console.ReadLine();
            cmd.Parameters.AddWithValue("@title", title);
            string content = Console.ReadLine();
            cmd.Parameters.AddWithValue("@content", content);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            Console.WriteLine("Note has just been inserted!");
            con.Close();
        }

        //Show all notes with database 
        static void ShowAllNotes(string userName)
        {
            // Tao database hoặc kết nôi database đã có theo ten nguoi dung
            string cs = @"URI=file:" + userName + ".db";
            using var con = new SQLiteConnection(cs);
            con.Open();

            //Read database
            //string stmRead = "SELECT * FROM Notes LIMIT 5";
            string stmRead = "SELECT * FROM Notes";
            using var cmdRead = new SQLiteCommand(stmRead, con);
            using SQLiteDataReader rdr = cmdRead.ExecuteReader();

            //Lấy tiêu đề bảng            
            Console.WriteLine($"{rdr.GetName(0),-3} {rdr.GetName(1),-20} {rdr.GetName(2),-20} {rdr.GetName(3),-10}");
            while (rdr.Read())
            {
                Console.WriteLine($"{rdr.GetInt32(0),-3} {rdr.GetString(1),-20} {rdr.GetString(2),-20} {rdr.GetString(3),-10}");
            }
            con.Close();
        }

        //get User name and check database
        static string GetUserName()
        {
            //Get user name        
            Console.WriteLine("Enter your Name...");
            string userName = Console.ReadLine();
            userName = userName.Replace(" ", "");
            //Kiểm tra database đã có chưa
            if (File.Exists(userName + ".db"))
            {
                //Neu co thi chao mung
                Console.WriteLine("Wellcome back! [" + userName+"]");
                return userName;
            }
            else
            {
                //Neu chua co, xac nhan lai voi ngoi dung xem co muon tao moi
                Console.WriteLine("User:[" + userName + "] is not Exist, create new user? [y/n]");
                if (Console.ReadKey(true).Key == ConsoleKey.Y)
                {
                    //bam y tao moi database theo Username
                    return userName;
                }
                else
                {
                    //bam n hoac cac phim khac thì chương trình sẽ hỏi lại Username
                    GetUserName();
                    return "default";
                }

            }

        }

        static void ShowNameApp()
        {
            string nameApp = @"
                     ██████╗ ██╗  ██╗██╗     ██████╗██╗  ██╗██╗   ██╗     █████╗ ██████╗ ██████╗ 
                    ██╔════╝ ██║  ██║██║    ██╔════╝██║  ██║██║   ██║    ██╔══██╗██╔══██╗██╔══██╗
                    ██║  ███╗███████║██║    ██║     ███████║██║   ██║    ███████║██████╔╝██████╔╝
                    ██║   ██║██╔══██║██║    ██║     ██╔══██║██║   ██║    ██╔══██║██╔═══╝ ██╔═══╝ 
                    ╚██████╔╝██║  ██║██║    ╚██████╗██║  ██║╚██████╔╝    ██║  ██║██║     ██║     
                     ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═════╝╚═╝  ╚═╝ ╚═════╝     ╚═╝  ╚═╝╚═╝     ╚═╝     
                                                                             
";
            Console.WriteLine(nameApp);
        }



    }
}
