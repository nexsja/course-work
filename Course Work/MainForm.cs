using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using Inventory.Entity;
using Inventory.Properties;

namespace Inventory
{
    public partial class MainForm : Form
    {
        private readonly Database _database;

        public MainForm(Database db)
        {
            _database = db;
            InitializeComponent();

            ForceGridUpdate();
        }

        private IList GetProductDataSource
        {
            get
            {
                return (from p in _database.ProductList
                    join v in _database.VendorList on p.VendorId equals v.VendorId
                    let vendorName = v.Name
                    let total = p.Quantity*p.Price
                    select new {p.Name, vendorName, p.Sku, p.Quantity, p.Price, total, p.Uom}
                    ).ToList();
            }
        }

        private void vendorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form vendorAddPopup = new VendorAddForm(_database);
            vendorAddPopup.Closed += ForceGridUpdate;
            vendorAddPopup.Show();
        }

        private void productToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form productAddPopup = new ProductAddForm(_database);
            productAddPopup.Closed += ForceGridUpdate;
            productAddPopup.Show();
        }

        //Actual method
        void ForceGridUpdate()
        {
            vendorListBox.DataSource = null;
            vendorListBox.DataSource = _database.VendorList;
            vendorListBox.DisplayMember = "Name";
            vendorListBox.ValueMember = "VendorId";

            dataGridProducts.DataSource = null;
            dataGridProducts.DataSource = GetProductDataSource;
        }

        //Required by EventHandler
        void ForceGridUpdate(object sender, EventArgs e)
        {
            ForceGridUpdate();
        }

        private void vendorListBox_MouseClick(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Right)
            {
                int selectedIndex = vendorListBox.IndexFromPoint(new Point(e.X, e.Y));
                vendorListBox.SelectedIndex = selectedIndex;

                if (selectedIndex == -1)
                {
                    return;
                }

                ContextMenu m = new ContextMenu();
                MenuItem item = new MenuItem("Remove");
                item.Click += (s, args) =>
                {
                    Vendor selectedVendor = (Vendor)vendorListBox.Items[selectedIndex];
                    int c = _database.ProductList.Count(p => p.VendorId == selectedVendor.VendorId);
                    if (c > 0)
                    {
                        DialogResult result = MessageBox.Show(
                            Resources.message_vendor_remove_products,
                            Resources.title_caution, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation
                        );

                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }
                    }

                    _database.VendorList.Remove(selectedVendor);
                    _database.ProductList.RemoveAll(p => p.VendorId == selectedVendor.VendorId);

                    _database.Save();
                    ForceGridUpdate();
                };                
                m.MenuItems.Add(item);
                m.Show(vendorListBox, new Point(e.X, e.Y));
            }
        }
    }
}
