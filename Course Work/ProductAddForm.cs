using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Inventory.Entity;

namespace Inventory
{
    public partial class ProductAddForm : Form
    {
        private Database _database;

        public ProductAddForm(Database db)
        {
            _database = db;
            InitializeComponent();

            //This is cool, but unnecessary
            //Dictionary<string, string> vendors = _database.VendorList.ToDictionary(v => v.VendorId, v => v.Name);

            vendorsCb.DataSource = _database.VendorList;
            vendorsCb.DisplayMember = "Name";
            vendorsCb.ValueMember = "VendorId";
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            //The initial idea was to do entity mapping directly, but there's no benifit to this here
            //except arbitraty complexity, but I'll leave this here for further reference.
            //Vendor vendor = _database.VendorList.Single(v => v.VendorId == vendorsCb.SelectedValue);

            string sku = skuBox.Text;
            string name = nameBox.Text;
            string uom = uomCb.Text;
            int quantity = Convert.ToInt16(qtyBox.Value);

            double price;
            Double.TryParse(priceBox.Text, out price);
            string vendorId = (string) vendorsCb.SelectedValue;

            Product product = new Product(sku, name, uom, quantity, vendorId, price);
            if (!product.IsValid)
            {
                MessageBox.Show("Product is not valid", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _database.ProductList.Add(product);
            _database.Save();
            Close();
        }
    }
}
