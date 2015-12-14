using System;
using System.Windows.Forms;
using Inventory.Forms;
using Inventory.Services;

namespace Inventory
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Database db = new Database();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(db));
        }
    }
}
