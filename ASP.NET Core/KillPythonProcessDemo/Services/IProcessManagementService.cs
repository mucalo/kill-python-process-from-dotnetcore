using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KillPythonProcessDemo.Services
{
    public interface IProcessManagementService
    {
        /// <summary>
        /// Method starts a system process runnig the file provided (should be an .exe file).
        /// </summary>
        /// <param name="filename">file to be run</param>
        /// <returns>system ID assigned by the system</returns>
        int StartNewProcess(string filename);

        /// <summary>
        /// Method kills a system process running
        /// </summary>
        /// <param name="systemId">system ID of the process to be killed</param>
        void KillProcess(int systemId);
    }
}
