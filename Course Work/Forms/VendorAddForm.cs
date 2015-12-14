using System.Windows.Forms;
using Inventory.Entity;
using Inventory.Properties;
using Inventory.Services;

namespace Inventory.Forms
{
    public partial class VendorAddForm : Form
    {
        private readonly Database _database;

        public Vendor EditVendor { set; private get; }

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

            Vendor vendor = EditVendor ?? new Vendor();
            vendor.Name = name;
            vendor.Address = address;

            _database.AddVendor(vendor);
            _database.Save();

            Close();
        }

        private void VendorAddForm_Load(object sender, System.EventArgs e)
        {
            if (EditVendor == null)
            {
                return;
            }

            textBox1.Text = EditVendor.Name;
            richTextBox1.Text = EditVendor.Address;
            button1.Text = Resources.button_edit;
        }
    }
}
