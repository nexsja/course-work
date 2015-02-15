namespace Inventory
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.vendorListBox = new System.Windows.Forms.ListBox();
            this.vendorListFilter = new System.Windows.Forms.TextBox();
            this.dataGridProducts = new System.Windows.Forms.DataGridView();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.vendorAddressBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridProducts)).BeginInit();
            this.SuspendLayout();
            // 
            // vendorListBox
            // 
            this.vendorListBox.FormattingEnabled = true;
            this.vendorListBox.Location = new System.Drawing.Point(523, 52);
            this.vendorListBox.Name = "vendorListBox";
            this.vendorListBox.Size = new System.Drawing.Size(120, 160);
            this.vendorListBox.TabIndex = 4;
            this.vendorListBox.SelectedIndexChanged += new System.EventHandler(this.VendorListSelectItem);
            this.vendorListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.vendorListBox_MouseDown);
            // 
            // vendorListFilter
            // 
            this.vendorListFilter.Location = new System.Drawing.Point(523, 26);
            this.vendorListFilter.Name = "vendorListFilter";
            this.vendorListFilter.Size = new System.Drawing.Size(120, 20);
            this.vendorListFilter.TabIndex = 5;
            this.vendorListFilter.TextChanged += new System.EventHandler(this.vendorListFilter_TextChanged);
            // 
            // dataGridProducts
            // 
            this.dataGridProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridProducts.Location = new System.Drawing.Point(12, 21);
            this.dataGridProducts.Name = "dataGridProducts";
            this.dataGridProducts.Size = new System.Drawing.Size(462, 191);
            this.dataGridProducts.TabIndex = 0;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3});
            this.menuItem1.Text = "Add";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "Vendor";
            this.menuItem2.Click += new System.EventHandler(this.VendorAddMenuItemClick);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "Product";
            this.menuItem3.Click += new System.EventHandler(this.ProductAddMenuItemClick);
            // 
            // vendorAddressBox
            // 
            this.vendorAddressBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.vendorAddressBox.Location = new System.Drawing.Point(649, 52);
            this.vendorAddressBox.Name = "vendorAddressBox";
            this.vendorAddressBox.ReadOnly = true;
            this.vendorAddressBox.Size = new System.Drawing.Size(152, 128);
            this.vendorAddressBox.TabIndex = 6;
            this.vendorAddressBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(646, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Vendor address:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(523, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Vendor filter:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 290);
            this.Controls.Add(this.dataGridProducts);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.vendorAddressBox);
            this.Controls.Add(this.vendorListFilter);
            this.Controls.Add(this.vendorListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.Text = "Inventory";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridProducts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox vendorListBox;
        private System.Windows.Forms.TextBox vendorListFilter;
        private System.Windows.Forms.DataGridView dataGridProducts;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.RichTextBox vendorAddressBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;




    }
}

