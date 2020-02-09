using System.Windows.Forms;

namespace Irvin.Extensions.Windows.Forms
{
    public interface IFormsHelper
    {
        DialogResult ShowMessageBox(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon, IWin32Window owner = null);
        void ExitApplication();
    }
}