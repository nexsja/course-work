using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Inventory.Entity;

namespace Inventory
{
    [Serializable()]
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
            if (_vendorList.Exists(v => v.VendorId == vendor.VendorId))
            {
                int index = _vendorList.FindIndex(v => v.VendorId == vendor.VendorId);
                _vendorList[index] = vendor;
            }
            else
            {
                _vendorList.Add(vendor);
            }
        }

        public void AddProduct(Product product)
        {
            _productList.Add(product);
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
                Database db = (Database) serializer.Deserialize(stream);
                _vendorList = db.VendorList;
                _productList = db.ProductList;
                db = null;
                stream.Close();
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
