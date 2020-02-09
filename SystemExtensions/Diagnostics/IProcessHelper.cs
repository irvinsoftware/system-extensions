using System.Collections.Generic;

namespace System.Diagnostics
{
    public interface IProcessHelper
    {
        string CurrentExecutableName { get; }
        Process Start(ProcessStartInfo startInfo);
        List<uint?> GetAllProcessesForUserByName(string processName, string currentUserFullName = null);
    }
}