using CommandControl_Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Command> pendingCommands = new List<Command>();
        private Timer timer = new Timer(2000);
        private StringBuilder executionResult = new StringBuilder();
        //private bool finished = false;
        private Client stored_client = null;

        public MainWindow()
        {
            InitializeComponent();
            timer.Enabled = false;
            timer.Elapsed += OnTimedEvent;
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    CommandLineReader clr = new CommandLineReader();
        //    clr.LaunchProcess("ipconfig", "", new CommandLineReader.CommandPassthorugh(Clr_commandPassThrough), new CommandLineReader.CommandPassthorugh(Clr_exitPassThrough));
        //}

        /// <summary>
        /// This event is being thrown from a thread, so needs to use the dispatcher
        /// </summary>
        /// <param name="data"></param>
        private void Clr_commandPassThrough(string data)
        {
            this.Dispatcher.Invoke((Action)(() =>
                executionResult.Append(data + " || ")));
        }

        private void Clr_exitPassThrough(string data)
        {
            this.Dispatcher.Invoke((Action)(() =>
                Send()));
        }

        //private void BtnPowershell_Click(object sender, RoutedEventArgs e)
        //{
        //    PowerShellReader clr = new PowerShellReader();
        //    clr.LaunchProcess("ipconfig", "", new PowerShellReader.CommandPassthorugh(Clr_commandPassThrough));
        //}

        private void BtnTrigger_Click(object sender, RoutedEventArgs e)
        {
            var webclient = new WebClient();
            webclient.Headers.Add(HttpRequestHeader.UserAgent, ".NET Framework Test Client");
            webclient.Headers.Add(HttpRequestHeader.ContentType, "text/json");

            Client client = new Client()
            {
                ClientName = Environment.MachineName
            };

            var s = JsonConvert.SerializeObject(client);
            try
            {
                string reply = webclient.UploadString(Settings.baseUrl + "/api/client/post", s);
                client = JsonConvert.DeserializeObject<Client>(reply);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            stored_client = client;

            List<Pending> pending = null; // will be created by the web process
            List<Command> commands = new List<Command>();

            try
            {
                var response = webclient.DownloadString(Settings.baseUrl + "/api/pending/get/" + client.Id.ToString());
                pending = JsonConvert.DeserializeObject<List<Pending>>(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            if ((pending != null) && (pending.Count > 0))
            {
                foreach (Pending p in pending)
                {
                    // get the command
                    var response = webclient.DownloadString(Settings.baseUrl + "/api/command/get/" + p.CommandId.ToString());
                    Command c = JsonConvert.DeserializeObject<Command>(response);
                    commands.Add(c);

                    // tell server client has seen the command
                    var p1 = JsonConvert.SerializeObject(new Pending { Id = p.Id });
                    response = webclient.UploadString(Settings.baseUrl + "/api/pending/post/" + p.Id.ToString(), p1);
                    var b = JsonConvert.DeserializeObject<int>(response);
                    if (b < 0)
                    {
                        MessageBox.Show("error?");
                    }

                    if (commands.Count > 0)
                    {
                        foreach (Command c1 in commands)
                        {
                            pendingCommands.Add(c1);
                        }

                        timer.Enabled = true;
                    }
                }
            }            
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            timer.Enabled = false;

            if (pendingCommands.Count > 0)
            {
                CommandLineReader clr = new CommandLineReader();
                clr.LaunchProcess(pendingCommands[0].scriptData, pendingCommands[0].scriptParameter, 
                    new CommandLineReader.CommandPassthorugh(Clr_commandPassThrough), 
                    new CommandLineReader.CommandPassthorugh(Clr_exitPassThrough));
                //finished = true;
            }
        }

        private void Send()
        {
            if (pendingCommands.Count > 0)
            {
                var p1 = JsonConvert.SerializeObject(new Execution {
                    ClientId = stored_client.Id,
                    CommandId = pendingCommands[0].Id,
                    Result = executionResult.ToString() });

                var webclient = new WebClient();
                webclient.Headers.Add(HttpRequestHeader.UserAgent, ".NET Framework Test Client");
                webclient.Headers.Add(HttpRequestHeader.ContentType, "text/json");

                try
                {
                    var response = webclient.UploadString(Settings.baseUrl + "/api/execution/post", p1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            pendingCommands.Remove(pendingCommands[0]);
            executionResult.Clear();

            timer.Enabled = true;
        }
    }
}
