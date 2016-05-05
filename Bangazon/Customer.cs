using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bangazon
{
    public class Customer
    {
        public int IdCustomer { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode
        {
            get
            {
                return PostalCode;
            }
            set
            {
                if (value.Length < 6)
                {
                    PostalCode = value;
                }
                else
                {
                    Console.WriteLine("Postal Code should be 5 digits");
                    Console.ReadKey();
                }
            }
        }

        public string PhoneNumber { get; set; }
    }
}
