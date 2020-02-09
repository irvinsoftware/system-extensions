using System.Collections.Generic;
using System.Management;

namespace System.Diagnostics
{
    public class ProcessHelper : IProcessHelper
    {
        public string CurrentExecutableName
        {
            get { return string.Format("{0}.exe", Process.GetCurrentProcess().ProcessName); }
        }

        public Process Start(ProcessStartInfo startInfo)
        {
            return Process.Start(startInfo);
        }

        //Derived from: http://stackoverflow.com/questions/566835/how-to-get-the-user-name-or-owner-of-a-process-in-net
        public List<uint?> GetAllProcessesForUserByName(string processName, string currentUserFullName = null)
        {
            List<uint?> matchingProcesses = new List<uint?>();

            string[] propertiesToSelect = new[] {"Handle", "ProcessId"};
            SelectQuery processQuery = new SelectQuery("Win32_Process", string.Format("Name = '{0}'", processName),
                                                       propertiesToSelect);
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(processQuery))
            {
                using (ManagementObjectCollection processes = searcher.Get())
                {
                    foreach (ManagementObject process in processes)
                    {
                        object[] outParameters = new object[2];
                        uint result = (uint) process.InvokeMethod("GetOwner", outParameters);

                        if (result == 0)
                        {
                            string user = (string) outParameters[0];
                            string domain = (string) outParameters[1];
                            uint processId = (uint) process["ProcessId"];

                            string processUserFullName = string.Format("{0}\\{1}", domain, user);
                            if (currentUserFullName == null ||
                                processUserFullName.Equals(currentUserFullName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                matchingProcesses.Add(processId);
                            }
                        }
                        else
                        {
                            matchingProcesses.Add(null);
                        }
                    }
                }
            }

            return matchingProcesses;
        }
    }
}