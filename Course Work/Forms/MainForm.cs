using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Inventory.Entity;
using Inventory.Properties;
using Inventory.Services;

namespace Inventory.Forms
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
                var products = (from p in _database.ProductList
                    join v in _database.VendorList on p.VendorId equals v.VendorId
                    let vendor = v.Name
                    let total = p.Quantity*p.Price
                    select new { p.ProductId, p.Name, vendor,  p.Sku, p.Quantity, p.Price, total, p.Uom }
                    );

                var field = "p.Sku";

                return products.OrderByDescending(p => field).ToList();



//                BindingList<Product> list = new BindingList<Product>(products);
//                return list;
            }
        }

        private void VendorAddMenuItemClick(object sender, EventArgs e)
        {
            FormOpener("VendorAddForm").Show();
        }

        private void ProductAddMenuItemClick(object sender, EventArgs e)
        {
            if (_database.VendorList.Count == 0)
            {
                MessageBox.Show("Please add vendors first", "No vendors", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            FormOpener("ProductAddForm").Show();
        }

        private Form FormOpener(string formName)
        {
            IList<Form> forms = Application.OpenForms.OfType<Form>().ToList();
            Form form = forms.SingleOrDefault(f => f.Name == formName);

            if (form != null)
            {
                form.WindowState = FormWindowState.Normal;
                form.Focus();
                return form;

            }

            form = (Form)Activator.CreateInstance(Type.GetType("Inventory.Forms." + formName), _database);
            form.Closed += ForceGridUpdate;
            return form;
        }

        private void SetVendorList(IList<Vendor> list)
        {
            vendorListBox.DataSource = null;
            vendorListBox.DataSource = list;
            vendorListBox.DisplayMember = "Name";
            vendorListBox.ValueMember = "VendorId";
        }

        //Actual method
        private void ForceGridUpdate()
        {
            SetVendorList(_database.VendorList);

            dataGridProducts.DataSource = null;
            dataGridProducts.DataSource = GetProductDataSource;
            dataGridProducts.Columns[0].Visible = false;
        }

        //Required by EventHandler
        private void ForceGridUpdate(object sender, EventArgs e)
        {
            ForceGridUpdate();
        }

        /**
         * Vendor list.
         * Provides Add/Edit/Remove functionality for vendors
         */
        private void vendorListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

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
                    VendorAddForm form = (VendorAddForm) FormOpener("VendorAddForm");
                    form.EditVendor = selectedVendor;
                    form.Show();

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

        /**
         * Filter vendors by name
         */
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

        /**
         * Double click to edit products
         */
        private void dataGridProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) //Ignore doubleclicks on headers
            {
                return;
            }

            string selectedProductId = (string) dataGridProducts.Rows[e.RowIndex].Cells[0].Value;
            ProductAddForm form = (ProductAddForm) FormOpener("ProductAddForm");
            form.EditProduct = _database.ProductList.Single(p => p.ProductId == selectedProductId);
            form.Show();
        }

        private void dataGridProducts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete)
            {
                return;
            }

            DialogResult btn = MessageBox.Show("Are you sure you want to delete this product?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (btn == DialogResult.Yes)
            {
                string productId = (string) dataGridProducts.CurrentRow.Cells[0].Value;
                _database.ProductList.RemoveAll(p => p.ProductId == productId);
                _database.Save();
                ForceGridUpdate();
            }
        }

        private void dataGridProducts_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridProducts.Sort(dataGridProducts.Columns[2], ListSortDirection.Descending);

            return;
            DataGridViewColumn newColumn = dataGridProducts.Columns[e.ColumnIndex];
            DataGridViewColumn oldColumn = dataGridProducts.SortedColumn;
            ListSortDirection direction;

            // If oldColumn is null, then the DataGridView is not currently sorted. 
            if (oldColumn != null)
            {
                // Sort the same column again, reversing the SortOrder. 
                if (oldColumn == newColumn &&
                    dataGridProducts.SortOrder == SortOrder.Ascending)
                {
                    direction = ListSortDirection.Descending;
                }
                else
                {
                    // Sort a new column and remove the old SortGlyph.
                    direction = ListSortDirection.Ascending;
                    oldColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
            }
            else
            {
                direction = ListSortDirection.Ascending;
            }

            dataGridProducts.Sort(newColumn, direction);
            newColumn.HeaderCell.SortGlyphDirection =
                direction == ListSortDirection.Ascending ?
                SortOrder.Ascending : SortOrder.Descending;
            
        }
    }
}
