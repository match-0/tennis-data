using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisData;

using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;
using System.Timers;
using System.Threading;
using System.IO;


namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Button3 clicked");
            var P = new Program();

            // Keep the program running
            while (true)
            {
                // Execute the function
                P.ExecuteFunction();

                // Wait for 24 hours before executing the function again
                //Thread.Sleep(TimeSpan.FromDays(1));

                // Wait for 10 seconds before executing the function again
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }

            Console.WriteLine("button3 ended");
        }

        private void ExecuteFunction()
        {
            Console.WriteLine("Function executed at: " + DateTime.Now);

            string currentTime = DateTime.Now.ToString();
            string timeLog = @"./time.txt";

            try
            {
                File.AppendAllText(timeLog, currentTime + "\n");
                Console.WriteLine("File wrote successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }





        }






    }
}
