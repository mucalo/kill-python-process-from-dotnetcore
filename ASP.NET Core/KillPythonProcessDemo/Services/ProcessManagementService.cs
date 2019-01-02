using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading.Tasks;


namespace KillPythonProcessDemo.Services
{
    public class ProcessManagementService : IProcessManagementService
    {
        public void KillProcess(int systemId)
        {
            KillProcessAndChildren(systemId);
            //Process p = Process.GetProcessById(systemId);
            //if(p != null)
            //{
            //    p.Kill();
            //}
        }

        public int StartNewProcess(string filename)
        {
            int sysId = -1;
            var p = new Process();
            p.StartInfo.FileName = filename;
            p.StartInfo.UseShellExecute = true;
            if(p.Start())
            {
                sysId = p.Id;
            }
            return sysId;
        }

        /// <summary>
        /// Kill a process, and all of its children, grandchildren, etc.
        /// </summary>
        /// <param name="pid">Process ID.</param>
        private static void KillProcessAndChildren(int pid)
        {
            // Cannot close 'system idle process'.
            if (pid == 0)
            {
                return;
            }
            ManagementObjectSearcher searcher = new ManagementObjectSearcher
                    ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                Process proc = Process.GetProcessById(pid);
                proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }
            catch (Win32Exception)
            {
                // Access denied
            }
        }
    }
}
