using System.Diagnostics;
using System.Windows.Forms;
using Inventory.Entity;
using Inventory.Properties;

namespace Inventory
{
    public partial class VendorAddForm : Form
    {
        private readonly Database _database;

        private Vendor _vendor;
        public Vendor EditVendor 
        {
            set { _vendor = value; }
        }

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

            Vendor vendor = _vendor ?? new Vendor();
            vendor.Name = name;
            vendor.Address = address;

            _database.AddVendor(vendor);
            _database.Save();

            Close();
        }

        private void VendorAddForm_Load(object sender, System.EventArgs e)
        {
            if (_vendor == null)
            {
                return;
            }

            textBox1.Text = _vendor.Name;
            richTextBox1.Text = _vendor.Address;
            button1.Text = Resources.button_edit;
        }
    }
}
