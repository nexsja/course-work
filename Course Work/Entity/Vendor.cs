using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Inventory.Entity
{
    [Serializable]
    public class Vendor : ISerializable
    {
        private const int _ver = 1;

        [Browsable(true)]
        public string VendorId { get; set; }

        public string Name { get; set; }
        
        public string Address { get; set; }
        
        public Vendor(string name, string address)
        {
            Name = name;
            Address = address;
            VendorId = Guid.NewGuid().ToString();
        }

        public Vendor(SerializationInfo info, StreamingContext context)
        {
            switch (info.GetInt32("_ver"))
            {
                case 0:
                    VendorId = info.GetInt32("VendorId").ToString();
                    break;
                case 1:
                    VendorId = info.GetString("VendorId");
                    break;
            }

            Address = info.GetString("Address");
            Name = info.GetString("Name");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_ver", _ver);
            info.AddValue("VendorId", VendorId);
            info.AddValue("Name", Name);
            info.AddValue("Address", Address);
        }
    }
}
