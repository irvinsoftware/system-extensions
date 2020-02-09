using System.Collections.Generic;
using System.Diagnostics;

namespace Irvin.Extensions.Diagnostics
{
    public interface IProcessHelper
    {
        string CurrentExecutableName { get; }
        Process Start(ProcessStartInfo startInfo);
        List<uint?> GetAllProcessesForUserByName(string processName, string currentUserFullName = null);
    }
}