using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractWebApp.Models
{
    public class Order
    {
        public string Id { get; set; }

        public string Sku { get; set; }

        public string ManufacturerId { get; set; }

        public int Quantity { get; set; }

        public string ContractAddress { get; set; }
    }
}
