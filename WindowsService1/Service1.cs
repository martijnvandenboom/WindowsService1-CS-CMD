using System;
using System.ServiceProcess;
using System.Timers;
using System.Diagnostics;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer;

        public Service1()
        {
            //InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Set up a timer to run the command every 5 minutes (adjust as needed)
            timer = new Timer();
            timer.Interval = 1 * 10 * 1000; // 5 minutes
            timer.Elapsed += OnTimerElapsed;
            timer.Start();
        }

        protected override void OnStop()
        {
            // Stop the timer when the service is stopped
            timer.Stop();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Execute your command here
            ExecuteCommand("C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe -ExecutionPolicy Bypass -File C:\\Windows\\Temp\\PowerShell\\MyScript001.ps1");
        }

        private void ExecuteCommand(string command)
        {
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
                //ProcessStartInfo processStartInfo = new ProcessStartInfo(command);
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;

                using (Process process = new Process())
                {
                    process.StartInfo = processStartInfo;
                    process.Start();
                    process.WaitForExit();
                    process.Close();
                    // You can also log the output if needed:
                    // string output = process.StandardOutput.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                // Log or display the exception message
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}