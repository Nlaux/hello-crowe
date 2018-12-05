using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Hello_World
{
    class Program_Backup
    {
        static void Main(string[] args)
        {
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

            string key = Console.ReadLine();

            //Make decision based on user selection
            bool confirmed = false;
            while (!confirmed)
            {
               
                if (key == "1")
                {
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

                    Console.WriteLine("writing to text file...");

                    File.WriteAllLines(filePath, output);

                    confirmed = true;
                } else if (key == "2")
                {
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
                    confirmed = true;
                }

            }


            








   
            // Read a particular key from the config file            
            //sAttr = ConfigurationManager.AppSettings.Get("Key0");
            //Console.WriteLine("The value of Key0: " + sAttr);

            //// Read all the keys from the config file
            //NameValueCollection sAll;
            //sAll = ConfigurationManager.AppSettings;

            //foreach (string s in sAll.AllKeys)
            //    Console.WriteLine("Key: " + s + " Value: " + sAll.Get(s));
            //Console.ReadLine();
        }
    }
}
