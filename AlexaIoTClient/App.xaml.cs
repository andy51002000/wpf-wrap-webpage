using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AlexaIoTClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        void App_Startup(object sender, StartupEventArgs e)
        {
            // Application is running
            // Process command line args
            bool startMinimized = false;
            for (int i = 0; i != e.Args.Length; ++i)
            {
                if (e.Args[i] == "/StartMinimized")
                {
                    startMinimized = true;
                }
            }

            var mediaPlayerHelper = new Helper.MediaPlayerHelper();
            var thread = new Thread(
                () =>   {
                            //remove before creating
                            mediaPlayerHelper.RemovePlayList();
                            mediaPlayerHelper.CreatePlayList();

                        }
                );

            thread.Start();

            RunProc("cmd.exe", $"/c start /b {System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)}\\client.cmd");
            RunProc("cmd.exe", $"/c {System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)}\\iot.cmd");
            Thread.Sleep(1000);


            // Create main application window, starting minimized if specified
            MainWindow mainWindow = new MainWindow();
            if (startMinimized)
            {
                mainWindow.WindowState = WindowState.Minimized;
            }
            mainWindow.Show();
        }
        public void RunProc(string programPath, string arg)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = programPath;
            proc.StartInfo.Arguments = arg;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(programPath);
            proc.Start();
        }
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            StreamWriter sw = new StreamWriter("proc.log");


            foreach (Process kProcess in Process.GetProcesses())
            {
                try
                {
                    sw.WriteLine(kProcess.Handle + "," + kProcess.ProcessName);
                    sw.Flush();

                    if (kProcess.ProcessName.StartsWith("cmd"))
                        kProcess.Kill();
                    if (kProcess.ProcessName.StartsWith("node"))
                        kProcess.Kill();
                }
                catch { }
            }
            sw.Close();
            KillCmdAsync();
            RunProc("taskkill", " /f /im node.exe");

        }

        public void KillCmd()
        {
            Array.ForEach(Process.GetProcessesByName("cmd"), x => x.Kill());
            Array.ForEach(Process.GetProcessesByName("mode"), x => x.Kill());
        }

        public async void KillCmdAsync()
        {
            await Task.Run(() => KillCmd());
        }



    }
}
