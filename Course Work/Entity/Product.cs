using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace Inventory.Entity
{
    [Serializable()]
    public class Product
    {
        private readonly string _productId;

        public string ProductId
        {
            get { return _productId; }
        }

        public string Sku { set; get; }

        public string Name { get; set; }

        public string Uom { get; set; }

        public int Quantity { get; set; }

        public string VendorId { get; set; }

        public double Price { get; set; }

        public bool IsValid {
            get
            {
                bool valid = Sku.Length > 3; //@TODO: Sku must follow a format - Regexp.
                valid = valid && Name.Length > 3;
                valid = valid && Uom.Length > 0;
                valid = valid && Quantity > 0;
                valid = valid && Price >= 0; //Price cannot be negative
                return valid;
            }
        }

        public Product()
        {
            _productId = Guid.NewGuid().ToString();
        }

        public Product(string sku, string name, string uom, int quantity, string vendorId, double price)
        {
            _productId = Guid.NewGuid().ToString();
            Sku = sku;
            Name = name;
            Uom = uom;
            Quantity = quantity;
            VendorId = vendorId;
            Price = price;
        }
    }
}
