using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Collections.ObjectModel;

namespace DesktopClient
{
    public class PowerShellReader
    {
        public delegate void CommandPassthorugh(string data);
        private CommandPassthorugh cpass;

        public void LaunchProcess(string command, string parameters, CommandPassthorugh cpt)
        {
            cpass = cpt;

            System.Management.Automation.Runspaces.Runspace runspace = 
                System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace();
            runspace.Open();

            PowerShell ps = PowerShell.Create(); // Create a new PowerShell instance
            ps.Runspace = runspace; // Add the instance to the runspace
            ps.Commands.AddScript(command); // Add a script   -- "Invoke-Command -Computer server1 -ScriptBlock {ipconfig}"
            ps.Commands.AddStatement().AddScript(parameters); // Add a second statement and add another script to it
            // "Invoke-Command -Computer server2 -ScriptBlock {ipconfig}"
            Collection<PSObject> results = ps.Invoke();

            runspace.Close();

            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results)
            {
                cpass?.Invoke(obj.ToString());
            }
        }
    }
}
