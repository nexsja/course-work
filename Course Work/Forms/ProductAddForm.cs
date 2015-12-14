using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using System.Linq;
using Inventory.Entity;
using Inventory.Properties;
using Inventory.Services;

namespace Inventory.Forms
{
    public partial class ProductAddForm : Form
    {
        private readonly Database _database;

        public Product EditProduct { set; private get; }

        public ProductAddForm(Database db)
        {
            _database = db;

            //This is cool, but unnecessary
            //Dictionary<string, string> vendors = _database.VendorList.ToDictionary(v => v.VendorId, v => v.Name);

            InitializeComponent();

            uomCb.DataSource = _database.ProductList.GroupBy(p => p.Uom).Select(p => p.Key).ToList();
            vendorsCb.DataSource = _database.VendorList;
            vendorsCb.DisplayMember = "Name";
            vendorsCb.ValueMember = "VendorId";
        }

        private void button1_Click(object sender, EventArgs e)
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
            
            Product product = EditProduct ?? new Product();
            product.Sku = sku;
            product.Name = name;
            product.Uom = uom;
            product.Quantity = quantity;
            product.VendorId = vendorId;
            product.Price = price;

            try
            {
                Validator.ValidateObject(product, new ValidationContext(product), true);
                _database.AddProduct(product);
                _database.Save();
                Close();
            }
            catch (ValidationException exception)
            {
                MessageBox.Show("The following errors occurred: \n\n" + exception.Message, Resources.error_title, MessageBoxButtons.OK, MessageBoxIcon.Error);    
            }
        }

        private void ProductAddForm_Load(object sender, EventArgs e)
        {
            if (EditProduct == null)
            {
                return;
            }

            skuBox.Text = EditProduct.Sku;
            nameBox.Text = EditProduct.Name;
            uomCb.Text = EditProduct.Uom;
            qtyBox.Value = EditProduct.Quantity;
            priceBox.Text = Convert.ToString(EditProduct.Price);
            vendorsCb.SelectedValue = EditProduct.VendorId;

            button1.Text = Resources.button_edit;
            Text = "Editing " + EditProduct.Name + " (" + EditProduct.Sku + ")";
        }
    }
}
