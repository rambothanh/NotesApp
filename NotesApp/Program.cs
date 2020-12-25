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

            ShowGiaoDien(userName);

        }

        //Hiện giao diện lựa chọn tống của chương trình
            static void ShowGiaoDien(string userName)
            {
                Console.Clear();
                Console.WriteLine("Hi! ["+userName+"]");
                Console.WriteLine("Please select the functions according to the number:");
                Console.WriteLine("     [1]. Show all notes");
                Console.WriteLine("     [2]. Add a new note");
                Console.WriteLine("     [3]. Edit note");
                Console.WriteLine("     [4]. Delete note");
                Console.WriteLine("     [e]. Exit");
                    Console.Write("Your choice:");
                var choice = Console.ReadLine();
                if (choice == "1")
                {
                    ShowAllNotes(userName);
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadKey();
                    ShowGiaoDien(userName);
                }else if (choice == "2")
                {
                    AddANewNote(userName);
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadKey();
                    ShowGiaoDien(userName);
                }else if(choice == "3")
                {
                    EditNote(userName);
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadKey();
                    ShowGiaoDien(userName);                    
                }else if(choice == "4")
                {
                    DeleteNote(userName);
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadKey();
                    ShowGiaoDien(userName);                    
                }else if (choice == "e")
                {
                    Environment.Exit(0);
                }
            }
        //edit note
        static void EditNote(string userName)
        {
            ShowAllNotes(userName);
            Console.WriteLine("Press id number to edit note...");
            Console.Write("id:");
            var id = Console.ReadLine();
            Console.WriteLine("You are editing a note, are you sure? [y/n]");            
            if (Console.ReadKey(true).Key == ConsoleKey.Y)
            {
                string cs = @"URI=file:" + userName + ".db";
                using var con = new SQLiteConnection(cs);
                con.Open();
                using var cmd = new SQLiteCommand(con);
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS  Notes(id INTEGER PRIMARY KEY,
                        date TEXT,title TEXT, content TEXT)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "UPDATE Notes SET title = @title, content = @content WHERE id = @id";
                Console.WriteLine("Editing a note id=["+id+"]...");
                cmd.Parameters.AddWithValue("@id", id);
                Console.Write("Title:");
                string newTitle = Console.ReadLine();
                cmd.Parameters.AddWithValue("@title", newTitle);
                Console.Write("Content:");
                string newContent = Console.ReadLine();
                cmd.Parameters.AddWithValue("@content", newContent);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Note id=["+id+"] has just been edited!");
                con.Close();   
            }else{ShowGiaoDien(userName);}            
        }

        //Delete Note
        static void DeleteNote(string userName)
        {
            ShowAllNotes(userName);
            Console.WriteLine("Press id number to delete...");
            Console.Write("id:");
            var id = Console.ReadLine();
            Console.WriteLine("You are deleting a note, are you sure? [y/n]");
            if (Console.ReadKey(true).Key == ConsoleKey.Y)
            {
                    //bam y Dong y delete
                    string cs = @"URI=file:" + userName + ".db";
                    using var con = new SQLiteConnection(cs);
                    con.Open();
                    using var cmd = new SQLiteCommand(con);
                    cmd.CommandText = @"CREATE TABLE IF NOT EXISTS  Notes(id INTEGER PRIMARY KEY,
                            date TEXT,title TEXT, content TEXT)";
                    cmd.ExecuteNonQuery();
                    //cmd.CommandText = "INSERT INTO Notes(date, title,content) VALUES(datetime('now', 'localtime'), @title, @content)";
                    cmd.CommandText = "DELETE FROM Notes WHERE ID = @id";
                    cmd.Parameters.AddWithValue("@id",id);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Note has just been deleted!");
                    con.Close();     
            }
            else
            {
                  ShowGiaoDien(userName);
            }
       
        }


        //Hỏi người dùng add thêm Note
        static void AddANewNote(string userName)
        {
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
            //
            Console.WriteLine("Adding new note...");
            Console.Write("Title:");
            string title = Console.ReadLine();
            cmd.Parameters.AddWithValue("@title", title);
            Console.Write("Content:");
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
            //Xóa màn hình cho đỡ rối
            Console.Clear();
            // Tao database hoặc kết nôi database đã có theo ten nguoi dung
            string cs = @"URI=file:" + userName + ".db";
            using var con = new SQLiteConnection(cs);
            con.Open();
            //Kiểm tra table nếu chưa có thì tạo mới
            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS  Notes(id INTEGER PRIMARY KEY,
                    date TEXT,title TEXT, content TEXT)";
            cmd.ExecuteNonQuery();
            //Read database
            //string stmRead = "SELECT * FROM Notes LIMIT 5";
            string stmRead = "SELECT * FROM Notes";
            using var cmdRead = new SQLiteCommand(stmRead, con);
            using SQLiteDataReader rdr = cmdRead.ExecuteReader();

            Console.WriteLine("        ALL NOTE(s) OF ["+userName+"]");
            //Lấy tiêu đề bảng            
            Console.WriteLine($"{rdr.GetName(0),-3} {rdr.GetName(1),-19} {rdr.GetName(2),-20} {rdr.GetName(3),-30}");
            while (rdr.Read())
            {
                //số -3, -20  -30 là độ rộng, dấu trừ là canh trái
                //Hiện id và date trước:
                Console.Write($"{rdr.GetInt32(0),-3} {rdr.GetString(1),-19} ");
                //Hiện title và content thông minh nhiều dòng thông minh.
                var titleString = rdr.GetString(2);
                var titleLen = titleString.Length;
                var soDong = titleLen/20; 
                for (int i = 0; i <= soDong; i++){
                    if (i==0){
                        //Hiện dòng đầu tiên
                        //Console.WriteLine(soDong);
                        Console.WriteLine($"{titleString.Substring(0,Math.Min(titleLen,20)),-20}");
                    }else{
                        //Từ dòng thứ 2 trở đi cách đầu dòng 24 ký tự
                        Console.Write("                        ");
                        //Substring(vịtrí, độdài)
                        Console.WriteLine($"{titleString.Substring(20*i,Math.Min(titleLen - 20*i,20)),-20}");
                        //Console.WriteLine($"{titleString.Substring(i*20,Math.Min(titleLen,i*20+20)),-20}");
                    }

                }
                //Console.WriteLine($"{titleString.Substring(1,20),-20}");
                //Hiện title và Content một dòng đơn giản
                //Console.WriteLine($"{rdr.GetString(2),-20} {rdr.GetString(3),-30} ");
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
                Console.WriteLine("Wellcome back! [" + userName + "]");
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
