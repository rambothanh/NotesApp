using System;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace NotesApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ShowNameApp();
            GetAllDb();
            
            //show name app
            // ShowNameApp();

            // //Lay ten nguoi dung và kiểm tra tồn tại, nếu không tồn tại hỏi người dùng 
            // //có muốn tạo mới, không tạo mới thì đánh lại tên người dùng khác
            

            

        }
        // GetAll Database in current Directory
        //
        static void GetAllDb(){
            Console.Clear();
            Console.WriteLine("All NoteBook(s) (Name or Database)");
            Console.WriteLine("-----------------------------------------"); 
            ProcessDirectory(Directory.GetCurrentDirectory());
            string userName = GetUserName();
            ShowGiaoDien(userName);

            static void ProcessDirectory(string targetDirectory)
            {
                // Process the list of files found in the directory.
                string [] fileEntries = Directory.GetFiles(targetDirectory,"*.db");
                foreach(string fileName in fileEntries)
                {
                    
                    ProcessFile(fileName);
                }
                // Recurse into subdirectories of this directory.
                // Không cần tìm trong thư mục con nên Comment phần này
                // string [] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
                // foreach(string subdirectory in subdirectoryEntries)
                //     ProcessDirectory(subdirectory);
            }
            // Insert logic for processing found files here.
            static void ProcessFile(string path)
            {
                Console.WriteLine(Path.GetFileName(path));	
            }
        }

        //Hiện giao diện lựa chọn tống của chương trình
        static void ShowGiaoDien(string userName)
            {
                Console.Clear();
                Console.WriteLine("Current Notebook: ["+userName+"]");
                Console.WriteLine("Please select the functions according to the number:");
                Console.WriteLine("     [1]. Show all Note(s)");
                Console.WriteLine("     [2]. Add a new note");
                Console.WriteLine("     [3]. Edit note");
                Console.WriteLine("     [4]. Delete note");
                Console.WriteLine("     [5]. Show all Notebook(s)");
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
                }else if(choice == "5")
                {
                    GetAllDb();                    
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
        //Show NUMBER OF RECORD (rOW) IN TABLE
        static int NumberOfRow(string userName){
            string cs = @"URI=file:" + userName + ".db";
            using var con = new SQLiteConnection(cs);
            con.Open();
            //Kiểm tra table nếu chưa có thì tạo mới
            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS  Notes(id INTEGER PRIMARY KEY,
                    date TEXT,title TEXT, content TEXT)";
            cmd.ExecuteNonQuery();
            //Đếm số lượng record
            //SELECT COUNT(DISTINCT id) FROM Notes;
            string stmCount = "SELECT COUNT(DISTINCT id) FROM Notes";
            using var cmdCount = new SQLiteCommand(stmCount,con);
            var countRecord = cmdCount.ExecuteScalar();
            //countRecord.Read();
            // var count  =  countRecord.GetInt32(0);
            int count = int.Parse(countRecord.ToString());
            con.Close();
            return (int)count;

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
            //Read database: giới hạn 5 record
            //string stmRead = "SELECT * FROM Notes LIMIT 5";
            //Read all record
            string stmRead = "SELECT * FROM Notes";
            using var cmdRead = new SQLiteCommand(stmRead, con);
            using SQLiteDataReader rdr = cmdRead.ExecuteReader();
            // //Đếm số lượng record
            var numberOfRow = NumberOfRow(userName);
            Console.WriteLine("        ALL NOTE(s) OF ["+userName+"]");
            // //Hiện tiêu đề bảng (không vẽ bảng)            
            // Console.WriteLine($"{rdr.GetName(0),-3} {rdr.GetName(1),-19} {rdr.GetName(2),-20} {rdr.GetName(3),-30}");
            // Hiện tiêu đề bảng có vẽ bảng:
            CreateTable(rdr.GetName(0).ToUpper(),rdr.GetName(1).ToUpper(),rdr.GetName(2).ToUpper(),rdr.GetName(3).ToUpper(), 0);
            //Xác định dòng cuối cùng
            Boolean dongCuoi = false;
            var row =0;
            while (rdr.Read())
            {
                
                //Hiện nội dung từng dòng, có vẽ bảng:
                //Đếm số dòng
                row++;
                if(row==numberOfRow){
                    //Xác định dòng cuối để vẽ đòng bảng lại
                    dongCuoi = true;
                }
                CreateTableMutiLine(rdr.GetInt32(0).ToString(),rdr.GetString(1),rdr.GetString(2),rdr.GetString(3), dongCuoi);

                // //Hiện nội dung không vẽ bảng
                // //số -3, -20  -30 là độ rộng, dấu trừ là canh trái
                // //Hiện id và date trước:
                // Console.Write($"{rdr.GetInt32(0),-3} {rdr.GetString(1),-19} ");
                
                // //Hiện title và content thông minh nhiều dòng
                // //Vẫn chưa kẻ bảng sẽ update kẻ bảng sau
                // var titleString = rdr.GetString(2);
                // var titleLen = titleString.Length;
                // var soDongTitle = titleLen/20; 
                // var contentString = rdr.GetString(3);
                // var contentLen = contentString.Length;
                // var soDongcontent = contentLen/30;                 
                // for (int i = 0; i <= Math.Max(soDongTitle,soDongcontent); i++){
                //     if (i==0){
                //         //Hiện dòng đầu tiên
                //         //Console.WriteLine(soDong);
                //         Console.Write($"{titleString.Substring(0,Math.Min(titleLen,20)),-20} ");
                //         Console.WriteLine($"{contentString.Substring(0,Math.Min(contentLen,30)),-30}");
                //     }else{
                //         if (i <=Math.Min(soDongTitle, soDongcontent))
                //         {
                //             //Từ dòng thứ 2 trở đi cách đầu dòng 24 ký tự
                //             Console.Write("                        ");
                //             //Hiển thị title 
                //             //Substring(vịtrí, độ dài)
                //             Console.Write($"{titleString.Substring(20 * i, Math.Min(titleLen - 20 * i, 20)),-20} ");
                //             Console.WriteLine($"{contentString.Substring(30 * i, Math.Min(contentLen - 30 * i, 30)),-30}");

                //         }else if (soDongTitle >= soDongcontent)
                //         {
                //             //Từ dòng thứ 2 trở đi cách đầu dòng 24 ký tự
                //             Console.Write("                        ");
                //             Console.WriteLine($"{titleString.Substring(20 * i, Math.Min(titleLen - 20 * i, 20)),-20} ");
                //         }else if (soDongTitle < soDongcontent)
                //         {
                //             //thêm 24 + 21 =45ký tự
                //             Console.Write("                                             ");
                //             Console.WriteLine($"{contentString.Substring(30 * i, Math.Min(contentLen - 30 * i, 30)),-30}");
                //         }

                //     }
                // }
                // //Console.WriteLine($"{titleString.Substring(1,20),-20}");
                // //Hiện title và Content một dòng đơn giản
                // //Console.WriteLine($"{rdr.GetString(2),-20} {rdr.GetString(3),-30} ");
            }
            con.Close();
        }

        //get User name and check database
        //UserName chính là NoteBook(s)
    static string GetUserName()
        {
            //Get user name, notebook, database       
            Console.WriteLine("-----------------------------------------"); 
            Console.WriteLine("Enter your NoteBook (Name or Database)...");
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
                    
                    return GetUserName();
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
            Console.WriteLine("Press enter to continue...");
            Console.ReadKey();

        }


    //Funtion vẽ bảng dành riêng cho project này
    //Vẽ bảng với title và content dài có nhiều dòng, bắt đầu từ dữ liệu, không có tiêu đề
    static void CreateTableMutiLine(string id, string date, string title, string content, Boolean bottomTable)
        {
            
                var lenTitle = title.Length;
                var lineTitle = lenTitle / 20;
                var lenContent = content.Length;
                var lineContent = lenContent / 30;
                var maxLine = Math.Max(lineTitle, lineContent);
                for (int i = 0; i <= maxLine; i++)
                {
                    //Vị trí bằng 3 là chưa chốt dòng
                    //vị trí bằng 1 là chốt dòng luôn để sang dòng mới
                    //vị trí bằng 2 là chốt bảng luôn
                    int viTri = 3;
                    

                    if (i == maxLine && bottomTable)
                    {
                        viTri = 2;
                    }else if(i == maxLine)
                    {
                        viTri = 1;
                    }

                    if (i == 0)
                    {
                        //Hiện dòng đầu tiên
                        CreateTable(id, date, title.Substring(0, Math.Min(lenTitle, 20)), content.Substring(0, Math.Min(lenContent, 30)), viTri);
                        
                    }
                    else
                    {
                        if (i <= Math.Min(lineTitle, lineContent))
                        {
                            CreateTable("", "", title.Substring(20 * i, Math.Min(lenTitle - 20 * i, 20)), content.Substring(30 * i, Math.Min(lenContent - 30 * i, 30)), viTri);
                           

                        }
                        else if (lineTitle >= lineContent)
                        {
                            CreateTable("", "", title.Substring(20 * i, Math.Min(lenTitle - 20 * i, 20)),"", viTri);
                            
                        }
                        else if (lineTitle < lineContent)
                        {
                            CreateTable("", "", "", content.Substring(30 * i, Math.Min(lenContent - 30 * i, 30)), viTri);
                            
                        }

                    }
                }
        }
    //Vẽ bảng một dòng
    static void CreateTable(string id, string date, string title, string content, int Vitri)
        {
            //Vitri 0: top table và chứa luôn tiêu đề rồi đóng lại luôn
            //1: middle table chứa dữ liệu ở giữa (chưa phải cuối cùng) và chốt bảng ở giữa,
            //2: bottom table chứa dữ liệu cuối cùng và chốt bảng
            //3: middle table chứa dữ liệu ở giữa (không chốt bảng giưa)
            const string TopLeftJoint = "┌";
            const string TopRightJoint = "┐";
            const string BottomLeftJoint = "└";
            const string BottomRightJoint = "┘";
            const string TopJoint = "┬";
            const string BottomJoint = "┴";
            const string LeftJoint = "├";
            const string MiddleJoint = "┼";
            const string RightJoint = "┤";
            const char HorizontalLine = '─';
            const string VerticalLine = "│";
            //Them khoan trắng
            id = AddSpace(id, 3);
            date = AddSpace(date, 19);
            title = AddSpace(title, 20);
            content = AddSpace(content, 30);
            StringBuilder tableSb = new StringBuilder();
            if (Vitri == 0)
            {
                //Top
                tableSb.Append(TopLeftJoint)
                    .Append(HorizontalLine, 3).Append(TopJoint)
                    .Append(HorizontalLine, 19).Append(TopJoint)
                    .Append(HorizontalLine, 20).Append(TopJoint)
                    .Append(HorizontalLine, 30)
                    .Append(TopRightJoint).AppendLine();
                //Chứa Tiêu đề
                tableSb.Append(VerticalLine)
                    .Append(id).Append(VerticalLine)
                    .Append(date).Append(VerticalLine)
                    .Append(title).Append(VerticalLine)
                    .Append(content).Append(VerticalLine)
                    .AppendLine();
                //bottom chốt ở giữa
                tableSb.Append(LeftJoint)
                    .Append(HorizontalLine, 3).Append(MiddleJoint)
                    .Append(HorizontalLine, 19).Append(MiddleJoint)
                    .Append(HorizontalLine, 20).Append(MiddleJoint)
                    .Append(HorizontalLine, 30)
                    .Append(RightJoint);
                Console.WriteLine(tableSb);

            }
            else if (Vitri == 1)
            {
                //Chứa dữ liệu
                tableSb.Append(VerticalLine)
                    .Append(id).Append(VerticalLine)
                    .Append(date).Append(VerticalLine)
                    .Append(title).Append(VerticalLine)
                    .Append(content).Append(VerticalLine)
                    .AppendLine();
                //bottom chốt ở giữa
                tableSb.Append(LeftJoint)
                    .Append(HorizontalLine, 3).Append(MiddleJoint)
                    .Append(HorizontalLine, 19).Append(MiddleJoint)
                    .Append(HorizontalLine, 20).Append(MiddleJoint)
                    .Append(HorizontalLine, 30)
                    .Append(RightJoint);

                Console.WriteLine(tableSb);
            }
            else if (Vitri == 2)
            {
                //Chứa dữ liệu
                tableSb.Append(VerticalLine)
                    .Append(id).Append(VerticalLine)
                    .Append(date).Append(VerticalLine)
                    .Append(title).Append(VerticalLine)
                    .Append(content).Append(VerticalLine)
                    .AppendLine();
                //bottom cuoi cung the last bottom
                tableSb.Append(BottomLeftJoint)
                    .Append(HorizontalLine, 3).Append(BottomJoint)
                    .Append(HorizontalLine, 19).Append(BottomJoint)
                    .Append(HorizontalLine, 20).Append(BottomJoint)
                    .Append(HorizontalLine, 30)
                    .Append(BottomRightJoint).AppendLine();
                //tableSb.Append(BottomLeftJoint).Append(HorizontalLine, 10).Append(BottomRightJoint).AppendLine();
                Console.WriteLine(tableSb);
            }
            else if (Vitri == 3)
            {
                //Chứa dữ liệu
                tableSb.Append(VerticalLine)
                    .Append(id).Append(VerticalLine)
                    .Append(date).Append(VerticalLine)
                    .Append(title).Append(VerticalLine)
                    .Append(content).Append(VerticalLine);


                Console.WriteLine(tableSb);
            }
        }
        //thêm khoản trẳng cho vừa độ rộng
    static string AddSpace(string str,int doRong)
        {
            var strLen = str.Length;
            while ((doRong- strLen) >0)
            {
                str = str + " ";
                doRong--;
            }

            return str;
        }
    }
}
