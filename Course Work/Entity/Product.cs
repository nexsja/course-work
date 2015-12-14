using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Entity
{
    [Serializable]
    public class Product
    {
        private readonly string _productId;

        public string ProductId
        {
            get { return _productId; }
        }

        [Required, RegularExpression(@"^[A-Z0-9]{2}-[A-Z0-9]{2}-[A-Z0-9]+$", ErrorMessage = "Only letters and numbers are allowed in SKU. They must match this pattern: XX-XX-XXXX")]
        public string Sku { get; set; }

        [Required, MinLength(3, ErrorMessage = "Name must bet at least 3 characters long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Type in a unit of measure")]
        public string Uom { get; set; }

        [Required, Range(1, 100, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required]
        public string VendorId { get; set; }

        [Required, CustomValidation(typeof(Product), "ValidetePrice")]
        public double Price { get; set; }

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

        /**
         * Custom validation method. It's kind of useles here because in this case Range works just as good
         */
        public static ValidationResult ValidetePrice(double price)
        {
            bool isValid = price > 0;

            if (isValid)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Price must be greater than 0");
        }
    }
}
