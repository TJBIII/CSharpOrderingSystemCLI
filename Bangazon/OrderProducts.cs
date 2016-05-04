using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangazon
{
    public class OrderProducts
    {
        public int IdOrderProducts { get; set; }
        public int IdProduct { get; set; }
        public int IdCustomerOrder { get; set; }
        public int IdCustomer { get; set; }
        public int Count { get; set; }
        public int CustomerCount { get; set; }
        public string Price { get; set; }
        public string Name { get; set; }
    }
}
