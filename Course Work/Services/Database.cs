using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Inventory.Entity;
using Inventory.Properties;

namespace Inventory.Services
{
    [Serializable]
    public class Database
    {
        private const string dbFile = "data.db";

        private List<Vendor> _vendorList = new List<Vendor>();
        private List<Product> _productList = new List<Product>();

        public List<Product> ProductList
        {
            get { return _productList; }
        }

        public List<Vendor> VendorList
        {
            get { return _vendorList; }
        }

        public Database()
        {
            Load();
        }

        public void AddVendor(Vendor vendor)
        {
            int index = _vendorList.FindIndex(v => v.VendorId == vendor.VendorId);
            if (index > 0)
            {
                _vendorList[index] = vendor;
            }
            else
            {
                _vendorList.Add(vendor);
            }
        }

        public void AddProduct(Product product)
        {
            int index = _productList.FindIndex(p => p.ProductId == product.ProductId);
            if (index == -1) //Product not found
            {
                _productList.Add(product);
            }
            else
            {
                _productList[index] = product;
            }
        }

        public void Load()
        {
            if (File.Exists(dbFile))
            {
                Stream stream = File.OpenRead(dbFile);
                if (stream.Length == 0)
                {
                    stream.Close();
                    return;
                }
                BinaryFormatter serializer = new BinaryFormatter();
                try
                {
                    Database db = (Database) serializer.Deserialize(stream);
                    _vendorList = db.VendorList;
                    _productList = db.ProductList;
                    stream.Close();
                }
                catch (SerializationException)
                {
                    stream.Close();
                    DialogResult result = MessageBox.Show(
                        "An error ocurred because the database file is corrupted or because the application was updated.\n\n" +
                        "Click OK to re-create the file or click Cancel to close the application.",
                        "There was an error processing the database file", 
                        MessageBoxButtons.OKCancel, 
                        MessageBoxIcon.Error
                    );

                    if (result == DialogResult.OK)
                    {
                        _productList = new List<Product>();
                        _vendorList = new List<Vendor>();
                        Save();
                    }
                    else
                    {
                        if (Application.MessageLoop)
                        {
                            Application.Exit();
                        }
                        else
                        {
                            Environment.Exit(1);
                        }
                    }                    
                }
            }
        }

        public void Save()
        {
            Stream stream = File.OpenWrite(dbFile);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(stream, this);
            stream.Close();
        }
    }
}
