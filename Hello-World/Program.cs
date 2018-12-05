using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Hello_World
{
    //Start of Unit testing example, not fully flushed out yet. Appologies.
    class Program
    {
        public object Assert { get; private set; }

        public interface IConsole
        {
            void WriteToConsole(string msg);
        }

        public class FakeConsole : IConsole
        {
            public bool IsCalled = false;

            public void WriteToConsole(string msg)
            {
                IsCalled = true;
            }
        }

        public class My_program
        {
            IConsole _consol;
            public My_program(IConsole consol)
            {
                if (consol != null)
                    _consol = consol;
            }
            public void Greet()
            {
                _consol.WriteToConsole("Hello Crowe!");
            }
        }

        //[TestMethod]
        public void MyProgramShouldDisplayHelloWorldToTheConsole()
        {
            //arrange

            var console = new FakeConsole
            {
                IsCalled = false
            };
            My_program program = new My_program(console);
            //act
            program.Greet();

            //assert
            //Assert.AreEquals(true, console.IsCalled, "console was not called to display the greeting");
        }


        public static void Main(string[] args)
        {
            //Start of Unit testing. Not fully implimented yet, sorry about that.
            //var console = new FakeConsole();
            //console.IsCalled = false;
            //My_program program = new My_program(console);
            //program.Greet();

            Console.WriteLine("Hello, Crowe!");
            Console.WriteLine("*------------------------------------*");
            Console.WriteLine("*-                                  -*");
            Console.WriteLine("*-     Future proofing example      -*");
            Console.WriteLine("*-    Gather input and write to:    -*");
            Console.WriteLine("*-                                  -*");
            Console.WriteLine("*-     1. For text file             -*");
            Console.WriteLine("*-     2. For Sql Database          -*");
            Console.WriteLine("*-                                  -*");
            Console.WriteLine("*------------------------------------*");

            Console.WriteLine(" Please make your selection...");

            string key = Console.ReadLine();

            bool confirmed = false;
            while (!confirmed)
            {
                if (key == "1")
                {
                    WriteToTextFile();
                    confirmed = true;
                }
                else if (key == "2")
                {
                    WriteToSqlDatabase();
                    confirmed = true;
                }
            }
        }



        private static void WriteToTextFile()
        {
            //Gather user input
            Console.WriteLine("Enter your first name");
            string fName = Console.ReadLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            Console.WriteLine("Enter your last name");
            string lName = Console.ReadLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);

            //get text file location from app.config file
            string filePath = ConfigurationManager.AppSettings.Get("filePath");

            //collect data
            List<Person> people = new List<Person>();
            List<string> lines = File.ReadAllLines(filePath).ToList();

            people.Add(new Person { FirstName = fName, LastName = lName });
            List<string> output = new List<string>();

            foreach (var person in people)
            {
                output.Add($"{ person.FirstName} { person.LastName }");
            }

            Console.WriteLine("writing to text file, press any key to continue...");
            Console.ReadKey(true);
            File.WriteAllLines(filePath, output);

        }

        private static void WriteToSqlDatabase()
        {
            //Gather user input
            Console.WriteLine("Enter your first name");
            string fName = Console.ReadLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            Console.WriteLine("Enter your last name");
            string lName = Console.ReadLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);

            //Read SQL connection string info from app.config
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Test"].ConnectionString;

            //prep data to be inserted into database
            string insertStmt = "INSERT INTO dbo.People(FirstName, LastName)" + "VALUES(@FirstName, @LastName)";

            //create connection
            using (SqlConnection conn = new SqlConnection(connectionString))

            //Insert into database
            using (SqlCommand cmd = new SqlCommand(insertStmt, conn))
            {
                // set up the command's parameters - to avoid SQl injection attacks
                cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = fName;
                cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = lName;

                // open connection, execute command, close connection
                conn.Open();
                int result = cmd.ExecuteNonQuery();
                conn.Close();
            }

            Console.WriteLine("Info written to database, press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
