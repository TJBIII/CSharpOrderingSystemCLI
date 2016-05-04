using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangazon
{
    public class PaymentOption
    {
        private string _accountNumber;
        public int IdPaymentOption { get; set; }
        public int IdCustomer { get; set; }
        public string Name { get; set; }
        public string AccountNumber
        {
            get
            {
                return _accountNumber;
            }

            set
            {
                if (value.Length > 12)
                {
                    Console.WriteLine("Account Number should be less than 12 digits");
                } else
                {
                    _accountNumber = value;
                }
            }
        }
        public int BangazonListId { get; set; }
    }
}
