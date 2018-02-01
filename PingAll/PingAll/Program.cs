using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;
using System.Diagnostics;

namespace PingAll
{
    class Program
    {
        static void Main(string[] args)
        {
            string machinesFileName = ConfigurationManager.AppSettings["machinesFileName"];

            // Read txt file from machines.txt
            string[] machines = File.ReadAllLines(machinesFileName);

            // foreach every machine, and ping the machine,
            foreach(string machine in machines)
            {
                string sCmdText = @"/C ping -a "+ machine;
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = sCmdText;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();

                string output = p.StandardOutput.ReadToEnd();
                //Console.WriteLine(machine + " output:" + output);

                // then show the result: Pass, Timeout, Unreachable, 
                // if show the text with Green, Yellow, Red color on the console, it will be better.
                if (output.Contains(@"Request timed out."))
                {
                    // Timeout
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(machine + " Timeout");
                }
                else if (output.Contains(@"request could not find host"))
                {
                    // Unreachable
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(machine + " Unreachable");
                }
                else if (output.Contains(@"(0% loss)"))
                {
                    // Pass
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(machine + " Pass");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(machine + " Didn't get status, output:" + output);
                }
            }
            Console.ResetColor();
            Console.WriteLine("Please press Enter key to exit the program.");
            Console.ReadKey();
        }
    }
}
