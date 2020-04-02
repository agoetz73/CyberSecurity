using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DesktopClient
{
    public class CommandLineReader
    {

        Process process = new Process();

        public delegate void CommandPassthorugh(string data);
        private CommandPassthorugh cpass;
        private CommandPassthorugh epass;

        public void LaunchProcess(string command, string parameters, CommandPassthorugh cpt, CommandPassthorugh ept = null)
        {
            cpass = cpt;
            if (ept == null)
                epass = cpt;
            else
                epass = ept;

            process.EnableRaisingEvents = true;
            process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(process_OutputDataReceived);
            process.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(process_ErrorDataReceived);
            process.Exited += new System.EventHandler(process_Exited);

            process.StartInfo.FileName = "CMD.EXE";
            process.StartInfo.Arguments = "/C " + command;//parameters;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            //below line is optional if we want a blocking call
            //process.WaitForExit();
        }

        void process_Exited(object sender, EventArgs e)
        {
            //Console.WriteLine(string.Format("process exited with code {0}\n", process.ExitCode.ToString()));
            System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke((Action)(() =>
            {
                epass?.Invoke(string.Format("{0}", process.ExitCode.ToString()));
            }));
        }

        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            //Console.WriteLine(e.Data + "\n");
            System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke((Action)(() =>
            {
                cpass?.Invoke(e.Data);
            }));
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            //Console.WriteLine(e.Data + "\n");
            System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke((Action)(() =>
            {
                cpass?.Invoke(e.Data);
            }));            
        }
    }
}
