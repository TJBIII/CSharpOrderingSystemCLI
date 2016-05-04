using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangazon
{
    public class Menu
    {
        SqlData sqlData = new SqlData();

        List<int> Cart = new List<int>(); // initialize a cart with 0 items

        //constructor
        public Menu()
        {
            ShowMenu();
        }

        //show initial menu
        public void ShowMenu()
        {
            Console.Clear();
            Console.Write(
@"
*********************************************************
* * Welcome to Bangazon! Command Line Ordering System * *
*********************************************************
1.Create an account
2.Create a payment option
3.Order a product
4.Complete an order
5.See product popularity
6.Leave Bangazon!
>>> ");
            char userSelection = Console.ReadKey().KeyChar;
            Console.Clear();

            switch (userSelection)
            {
                case '1':
                    CreateNewCustomer();
                    break;
                case '2':
                    AddPaymentOption();
                    break;
                case '3':
                    OrderProducts();
                    break;
                case '4':
                    CompleteOrder();
                    break;
                case '5':
                    SeeProductPopularity();
                    break;
                case '6':
                    System.Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }

        public void ShowCustomerList()
        {

        }

        public void CreateNewCustomer()
        {

        }

        public void AddPaymentOption()
        {

        }

        public void OrderProducts()
        {

        }

        public void CompleteOrder()
        {

        }

        public void SeeProductPopularity()
        {

        }
    }
}
