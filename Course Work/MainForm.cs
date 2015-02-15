using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Inventory.Entity;
using Inventory.Properties;

namespace Inventory
{
    public partial class MainForm : Form
    {
        private readonly Database _database;

        private delegate Form FormWindowStateNormalizer(Database db);

        // Prevent duplicating form windows
        private VendorAddForm _vendorAddFormInstance;
        
        // Prevent duplicating form windows
        private ProductAddForm _productAddFormInstance;

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
                    select new { p.Name, vendorName, p.Sku, p.Quantity, p.Price, total, p.Uom }
                    ).ToList();
            }
        }

        private void VendorAddMenuItemClick(object sender, EventArgs e)
        {
            if (_vendorAddFormInstance != null)
            {
                _vendorAddFormInstance.WindowState = FormWindowState.Normal;
                _vendorAddFormInstance.Focus();
            }
            else
            {
                _vendorAddFormInstance = new VendorAddForm(_database);
                _vendorAddFormInstance.Closed += ForceGridUpdate;
                _vendorAddFormInstance.Closed += (o, args) => { _vendorAddFormInstance = null; };
                _vendorAddFormInstance.Show();
            }
        }

        private void ProductAddMenuItemClick(object sender, EventArgs e)
        {
            if (_productAddFormInstance != null)
            {
                _productAddFormInstance.WindowState = FormWindowState.Normal;
                _productAddFormInstance.Focus();
            }
            else
            {
                _productAddFormInstance = new ProductAddForm(_database);
                _productAddFormInstance.Closed += ForceGridUpdate;
                _productAddFormInstance.Closed += (o, args) => { _productAddFormInstance = null; };
                _productAddFormInstance.Show();
            }
        }

        void SetVendorList(IList<Vendor> list)
        {
            vendorListBox.DataSource = null;
            vendorListBox.DataSource = list;
            vendorListBox.DisplayMember = "Name";
            vendorListBox.ValueMember = "VendorId";
        }

        //Actual method
        void ForceGridUpdate()
        {
            SetVendorList(_database.VendorList);

            dataGridProducts.DataSource = null;
            dataGridProducts.DataSource = GetProductDataSource;
        }

        //Required by EventHandler
        void ForceGridUpdate(object sender, EventArgs e)
        {
            ForceGridUpdate();
        }

        private void vendorListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            int selectedIndex = vendorListBox.IndexFromPoint(new Point(e.X, e.Y));
            vendorListBox.SelectedIndex = selectedIndex;

            ContextMenu m = new ContextMenu();

            MenuItem insertItem = new MenuItem("Insert");
            insertItem.Click += VendorAddMenuItemClick;

            m.MenuItems.Add(insertItem);

            if (selectedIndex > -1)
            {
                Vendor selectedVendor = (Vendor)vendorListBox.Items[selectedIndex];

                MenuItem editItem = new MenuItem("Edit");
                editItem.Click += (s, args) =>
                {
                    VendorAddForm vendorEdit = new VendorAddForm(_database) { EditVendor = selectedVendor };
                    vendorEdit.Closed += ForceGridUpdate;
                    vendorEdit.Show();

                };
                m.MenuItems.Add(editItem);

                MenuItem removeItem = new MenuItem("Remove");
                removeItem.Click += (s, args) =>
                {
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
                m.MenuItems.Add(removeItem);
            }

            
            m.Show(vendorListBox, new Point(e.X, e.Y));
        }

        private void VendorListSelectItem(object sender, EventArgs e)
        {
            if (vendorListBox.SelectedIndex == -1)
            {
                return;
            }
            Vendor vendor = (Vendor) vendorListBox.SelectedItem;
            vendorAddressBox.Text = vendor.Address;
        }

        private void vendorListFilter_TextChanged(object sender, EventArgs e)
        {
            string filter = vendorListFilter.Text.ToLower();
            if (filter.Length > 0)
            {
                IEnumerable<Vendor> result = _database.VendorList.Where(v => v.Name.ToLower().Contains(filter));
                var enumerable = result as IList<Vendor> ?? result.ToList();
                if (!enumerable.Any()) return;

                SetVendorList(enumerable);
            }
            else
            {
                ForceGridUpdate();
            }
        }
    }
}
