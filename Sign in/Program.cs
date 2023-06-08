using System;
using System.IO;

namespace Lesson_7_Homework
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Start();
        }
        public static void Start()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("[1] - Sign up\n[2] - Log in\n[3] - Delete user");
            int input;
            while (!int.TryParse(Console.ReadLine(), out input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input. Please enter a valid option.");
            }

            if (input == 1)
            {
                SignUp();
            }
            else if (input == 2)
            {
                LogIn();
            }
            else if (input == 3)
            {
                DeleteUser();
            }
        }

        public static void SignUp()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;

            Console.Write("Enter Full Name: ");
            string fullName = Console.ReadLine();
            Console.Write("Enter Username: ");
            string userName = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();
            Console.Write("Enter Phone Number: ");
            string phoneNumber = Console.ReadLine();

            string directoryPath = @"C:\Sign In\Registration";
            Directory.CreateDirectory(directoryPath);

            string filePath = Path.Combine(directoryPath, $"{userName}.txt");

            if (File.Exists(filePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("This account is already registered!");
            }
            else
            {
                User user = new User(userName, fullName, phoneNumber, password, filePath);
                user.SaveToFile();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("User registration successful!");
            }

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            Start();
        }

        public static void LogIn()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;

            Console.Write("Enter Username: ");
            string userName = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            string directoryPath = @"C:\Sign In\Registration";
            string filePath = Path.Combine(directoryPath, $"{userName}.txt");

            if (File.Exists(filePath))
            {
                User user = User.LoadFromFile(filePath);

                if (user.Password == password)
                {
                    Console.WriteLine("Username: " + user.UserName);
                    Console.WriteLine("Full Name: " + user.FullName);
                    Console.WriteLine("Phone: " + user.PhoneNumber);
                    Console.WriteLine("Password: " + user.Password);
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid password");
                }
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No such account exists");
                Console.WriteLine("First you should sign up");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                SignUp();
            }

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            Start();
        }

        public static void DeleteUser()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write("Enter Username to delete: ");
            string usernameInput = Console.ReadLine();
            Console.Write("Enter Password: ");
            string passwordInput = Console.ReadLine();

            string directoryPath = @"C:\Sign In\Registration";
            string filePath = Path.Combine(directoryPath, $"{usernameInput}.txt");

            try
            {
                if (File.Exists(filePath))
                {
                    User user = User.LoadFromFile(filePath);

                    if (user.Password == passwordInput)
                    {
                        user.Delete();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid password");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No such account exists");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            Start();
        }

        internal class User
        {
            public string UserName { get; set; }
            public string FullName { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
            public string FilePath { get; set; }

            public User(string userName, string fullName, string phoneNumber, string password, string filePath)
            {
                UserName = userName;
                FullName = fullName;
                PhoneNumber = phoneNumber;
                Password = password;
                FilePath = filePath;
            }

            public void SaveToFile()
            {
                using (StreamWriter sw = new StreamWriter(FilePath))
                {
                    sw.WriteLine("Username: " + UserName);
                    sw.WriteLine("Full Name: " + FullName);
                    sw.WriteLine("Phone: " + PhoneNumber);
                    sw.WriteLine("Password: " + Password);
                }
            }

            public static User LoadFromFile(string filePath)
            {
                string[] lines = File.ReadAllLines(filePath);

                string userName = lines[0].Substring("Username: ".Length);
                string fullName = lines[1].Substring("Full Name: ".Length);
                string phoneNumber = lines[2].Substring("Phone: ".Length);
                string password = lines[3].Substring("Password: ".Length);

                return new User(userName, fullName, phoneNumber, password, filePath);
            }

            public void Delete()
            {
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("User deleted successfully!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No such account exists");
                }
            }
        }
    }
}
