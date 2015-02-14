using System.Diagnostics;
using System.Windows.Forms;
using Inventory.Entity;
using Inventory.Properties;

namespace Inventory
{
    public partial class VendorAddForm : Form
    {
        private Database _database;

        public VendorAddForm()
        {
            InitializeComponent();
        }

        public VendorAddForm(Database db)
        {
            _database = db;
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            string name = textBox1.Text;
            string address = richTextBox1.Text;

            if (name.Length == 0 || address.Length == 0)
            {
                MessageBox.Show(Resources.error_name_address_empty, Resources.error_title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Vendor vendor = new Vendor(name, address);
            _database.AddVendor(vendor);
            _database.Save();

            Close();
        }
    }
}
