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
            Console.WriteLine("Who?");
            var allCustomers = sqlData.GetCustomers();
            foreach (var c in allCustomers)
            {
                Console.WriteLine("{0}: {1} {2}", c.IdCustomer, c.FirstName, c.LastName);
            }
            Console.WriteLine(">>>");
            Console.ReadKey();
        }

        public void CreateNewCustomer()
        {
            Console.Clear();
            var allc = sqlData.GetCustomers();
            var count = allc.Last().IdCustomer;

            Customer c = new Customer();
            c.IdCustomer = count + 1;
            Console.WriteLine("Enter first name \n>>>");
            c.FirstName = Console.ReadLine();
            Console.WriteLine("Enter last name \n>>>");
            c.LastName = Console.ReadLine();
            Console.WriteLine("Enter street address \n>>>");
            c.StreetAddress = Console.ReadLine();
            Console.WriteLine("Enter city \n>>>");
            c.City = Console.ReadLine();
            Console.WriteLine("Enter state \n>>>");
            c.State = Console.ReadLine();
            Console.WriteLine("Enter postal code \n>>>");
            c.PostalCode = Console.ReadLine();
            Console.WriteLine("Enter phone number \n>>>");
            c.PhoneNumber = Console.ReadLine();

            Console.WriteLine("Customer created!");
            sqlData.CreateCustomer(c);
            ShowMenu();
        }

        public void AddPaymentOption()
        {
            Console.Clear();
            ShowCustomerList();
            PaymentOption po = new PaymentOption();
            var id = Console.ReadLine();
            po.IdCustomer = Convert.ToInt32(id);

            Console.Write("Enter payment type \n>>>");
            po.Name = Console.ReadLine();

            Console.Write("Enter account number \n>>>");
            po.AccountNumber = Console.ReadLine();

            Console.WriteLine("Payment option created!");
            sqlData.CreatePaymentOption(po);
            ShowMenu();
        }

        public void OrderProducts()
        {
            Console.Clear();
            var products = sqlData.GetProducts();
            var exitVal = products.Last().IdProduct + 1;
            int? userInput = null;

            while (userInput != exitVal)
            {
                Console.WriteLine("What do you want:");
                foreach (var p in products)
                {
                    Console.WriteLine(p.IdProduct + ". " + p.Name);
                }
                Console.Write("... \n" + exitVal + ". Return to main menu \n");
                var stringInput = Console.ReadLine();
                userInput = Convert.ToInt32(stringInput);

                if (userInput != exitVal)
                {
                    Cart.Add((int)userInput);
                    Console.Clear();
                }
            }

            if (exitVal == userInput)
            {
                Console.Clear();
                ShowMenu();
            }
        }

        public void CompleteOrder()
        {
            Console.Clear();
            if (Cart.Count == 0)
            {
                Console.WriteLine("Nothing in your cart... Press any key to see the main menu.");
                Console.ReadKey();
                ShowMenu();
            }
            else
            {
                double total = 0;
                // get back cost of products for each Idproduct in cart
                foreach (var item in Cart)
                {
                    if (item != 6)
                    {
                        var products = sqlData.GetSingleProduct(item);
                        var price = Convert.ToDouble(products[0].Price);
                        total += price;
                    }
                }
                Console.Write("Your total is " + total + ". \n Ready to check out? \n[y/n] >>>");
                var readyToCheckout = Console.ReadLine();
                if (readyToCheckout.ToLower() == "n") ShowMenu();
                else if (readyToCheckout.ToLower() == "y") // only allow checkout on "Y"
                {
                    // choose a customer
                    CustomerOrder co = new CustomerOrder();
                    ShowCustomerList();
                    var strId = Console.ReadLine();
                    var custId = Convert.ToInt32(strId);

                    // get payment options
                    var paymentOptionsForCust = sqlData.GetPaymentOptions(custId);
                    Console.WriteLine("Choose a payment option: \n>>>");
                    foreach (var option in paymentOptionsForCust)
                    {
                        Console.WriteLine(option.IdPaymentOption + ". " + option.Name);
                    }
                    var strSelectedPayment = Console.ReadLine();
                    var selectedPayment = Convert.ToInt32(strSelectedPayment);

                    // get orders
                    int orderId = 1;
                    var currentOrders = sqlData.GetCustomerOrders();
                    if (currentOrders.Count != 0) orderId = currentOrders.Last().IdCustomerOrder + 1;

                    co.IdCustomerOrder = orderId;
                    co.IdPaymentOption = selectedPayment;
                    co.IdCustomer = custId;
                    co.OrderNumber = "ORDER NUMBER";
                    co.DateCreated = DateTime.Now.ToString();
                    co.PaymentType = "CREDIT";
                    co.Shipping = "FEDEX";

                    sqlData.CreateCustomerOrder(co);
                    // add each order product to OrderProducts
                    foreach (var item in Cart)
                    {
                        var currProducts = sqlData.GetOrderProducts();
                        var count = 0;
                        if (currProducts.Count != 0) count = currProducts.Last().IdOrderProducts;
                        OrderProducts op = new OrderProducts();
                        op.IdOrderProducts = count + 1;
                        op.IdProduct = item;
                        op.IdCustomerOrder = orderId;
                        op.IdCustomer = custId;
                        sqlData.CreateOrderProduct(op);
                    }

                    Console.WriteLine("Order Processed. Press any key.");
                    Console.ReadLine();
                    ShowMenu();
                }
            }
        }

        public void SeeProductPopularity()
        {
            Console.Clear();
            var productsOrdered = sqlData.GetOrderProductsCount();
            foreach (var p in productsOrdered)
            {
                var prodAttr = sqlData.GetSingleProduct(p.IdProduct);
                p.Name = prodAttr[0].Name;
                p.Price = prodAttr[0].Price;

                var custsPerProd = sqlData.GetCustomersPerProduct(p.IdProduct);
                p.CustomerCount = custsPerProd[0].CustomerCount;

                var total = Convert.ToDouble(p.Price) * p.Count;
                Console.WriteLine(p.Name + " purchased " + p.Count + " times by " + p.CustomerCount + " people for $" + total + " total.");
            }
            Console.WriteLine(">>> Press any key\n");
            Console.ReadLine();
            ShowMenu();

        }
    }
}
