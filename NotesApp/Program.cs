using System;

namespace NotesApp
{
    class Program
    {
        static void Main(string[] args)
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
            Console.Clear();
            Console.WriteLine("Enter your Name...");
            string userName = Console.ReadLine();
            Console.WriteLine("1. Show all notes");
            Console.WriteLine("2. Add a new note");
            Console.WriteLine("3. Edit note");
            Console.WriteLine("4. Delete note");
            Console.Write("Your choice:");
            string choice = Console.ReadLine();
            Console.WriteLine("ban da chon: " + choice);

        }



    }
}
