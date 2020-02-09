using System.Windows.Forms;

namespace Irvin.Extensions.Windows.Forms
{
    public class FormsHelper : IFormsHelper
    {
        public DialogResult ShowMessageBox(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon, IWin32Window owner = null)
        {
            return owner != null
                       ? MessageBox.Show(owner, message, title, buttons, icon)
                       : MessageBox.Show(message, title, buttons, icon);
        }

        public void ExitApplication()
        {
            Application.Exit();
        }

        private static IFormsHelper _helper;
        public static IFormsHelper Instance
        {
            get
            {
                if (_helper == null)
                {
                    _helper = new FormsHelper();
                }
                return _helper;
            }
        }
    }
}