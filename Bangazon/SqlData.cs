﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Bangazon
{
    public class SqlData
    {
        private SqlConnection _sqlConnection = new SqlConnection("Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename=C:\\Users\\TJB\\Documents\\workspaceW\\Bangazon2\\Bangazon\\BangazonStore.mdf;Integrated Security = True");

        public void CreateCustomer(Customer c)
        {
            //create a customer in the Customer table
            string command = String.Format(@"INSERT INTO Customer 
            (FirstName, LastName, StreetAddress, City, StateProvince, PostalCode, PhoneNumber, IdCustomer)
            VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')",
            c.FirstName, c.LastName, c.StreetAddress, c.City, c.State, c.PostalCode, c.PhoneNumber, c.IdCustomer);

            update(command);
        }

        public void CreatePaymentOption(PaymentOption po)
        {
            //create a payment option in PaymentOption table
            string command = String.Format(@"INSERT INTO PaymentOption 
            (IdPaymentOption, IdCustomer, Name, AccountNumber) 
            VALUES ('{0}', '{1}', '{2}', '{3}')",
            GetNextPaymentOptionId(), po.IdCustomer, po.Name, po.AccountNumber);

            update(command);
        }

        public int GetNextPaymentOptionId()
        {
            //Get the next unique id value for the PaymentOption table
                //should be able to navigate around this with table setup
            int nextId = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT MAX(IdPaymentOption) FROM PaymentOption";
            cmd.Connection = _sqlConnection;

            _sqlConnection.Open();
            using (SqlDataReader dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    nextId = dataReader.GetInt32(0);
                }
            }
            _sqlConnection.Close();

            return nextId + 1;
        }

        public void CreateOrderProduct(OrderProducts op)
        {
            //create entry in the OrderProducts table to link order/customer/customerorder
            string command = String.Format(@"INSERT INTO OrderProducts 
            (IdOrderProducts, IdProduct, IdCustomerOrder, IdCustomer)
            VALUES ({0}, {1}, {2}, {3})",
            op.IdOrderProducts, op.IdProduct, op.IdCustomerOrder, op.IdCustomer);

            update(command);
        }

        public void CreateCustomerOrder(CustomerOrder co)
        {
            //create entry in CustomerOrder table
            string command = String.Format(@"INSERT INTO CustomerOrder 
            (IdCustomerOrder, OrderNumber, DateCreated, IdCustomer, PaymentType, Shipping, IdPaymentOption)
            VALUES ({0}, '{1}', '{2}', {3}, '{4}', '{5}', {6})",
            co.IdCustomerOrder, co.OrderNumber, co.DateCreated, co.IdCustomer, co.PaymentType, co.Shipping, co.IdPaymentOption);

            update(command);
        }

        public List<Customer> GetCustomers()
        {
            //get list of all customers
            List<Customer> customerList = new List<Customer>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = @"SELECT IdCustomer, FirstName, LastName, StreetAddress, City, StateProvince, PostalCode, PhoneNumber
            FROM Customer 
            ORDER BY IdCustomer";

            cmd.Connection = _sqlConnection;

            _sqlConnection.Open();
            using (SqlDataReader dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    Customer customer = new Customer();
                    customer.IdCustomer = dataReader.GetInt32(0);
                    customer.FirstName = dataReader.GetString(1);
                    customer.LastName = dataReader.GetString(2);
                    customer.StreetAddress = dataReader.GetString(3);
                    customer.City = dataReader.GetString(4);
                    customer.State = dataReader.GetString(5);
                    customer.PostalCode = dataReader.GetString(6);
                    customer.PhoneNumber = dataReader.GetString(7);

                    customerList.Add(customer);
                }
            }
            _sqlConnection.Close();

            return customerList;
        }

        public List<Product> GetProducts()
        {
            //get list of all products
            List<Product> productList = new List<Product>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT IdProduct, Name, Description, Price, IdProductType FROM Product";
            cmd.Connection = _sqlConnection;

            _sqlConnection.Open();
            using (SqlDataReader dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    Product prod = new Product();
                    prod.IdProduct = dataReader.GetInt32(0);
                    prod.Name = dataReader.GetString(1);
                    prod.Description = dataReader.GetString(2);
                    prod.Price = dataReader.GetString(3);
                    prod.IdProductType = dataReader.GetInt32(4);

                    productList.Add(prod);
                }
            }
            _sqlConnection.Close();

            return productList;
        }

        public Product GetSingleProduct(int id)
        {
            //get the product that matches the id argument
            Product prod = new Product();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT IdProduct, Name, Description, Price, IdProductType FROM Product WHERE IdProduct = " + id;
            cmd.Connection = _sqlConnection;

            _sqlConnection.Open();
            using (SqlDataReader dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    prod.IdProduct = dataReader.GetInt32(0);
                    prod.Name = dataReader.GetString(1);
                    prod.Description = dataReader.GetString(2);
                    prod.Price = dataReader.GetString(3);
                    prod.IdProductType = dataReader.GetInt32(4);
                }
            }
            _sqlConnection.Close();

            return prod;
        }

        public List<OrderProducts> GetOrderProducts()
        {
            //get list of all OrderProducts table entries
            List<OrderProducts> orderProductsList = new List<OrderProducts>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT IdOrderProducts, IdProduct, IdCustomerOrder FROM OrderProducts";
            cmd.Connection = _sqlConnection;

            _sqlConnection.Open();
            using (SqlDataReader dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    OrderProducts op = new OrderProducts();
                    op.IdOrderProducts = dataReader.GetInt32(0);
                    op.IdProduct = dataReader.GetInt32(1);
                    op.IdCustomerOrder = dataReader.GetInt32(2);

                    orderProductsList.Add(op);
                }
            }
            _sqlConnection.Close();

            return orderProductsList;
        }

        public List<OrderProducts> GetOrderProductsCount()
        {
            //the number of times each product was ordered
            List<OrderProducts> orderProductsList = new List<OrderProducts>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = @"SELECT
            op.IdProduct,
            COUNT(op.IdProduct)
            FROM OrderProducts op
            GROUP BY op.IdProduct;";
            cmd.Connection = _sqlConnection;

            _sqlConnection.Open();
            using (SqlDataReader dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    OrderProducts op = new OrderProducts();
                    op.IdProduct = dataReader.GetInt32(0);
                    op.Count = dataReader.GetInt32(1);

                    orderProductsList.Add(op);
                }
            }
            _sqlConnection.Close();

            return orderProductsList;
        }

        public List<OrderProducts> GetCustomersPerProduct(int prodId)
        {
            //how many customers ordered each product
            List<OrderProducts> customersPerProductsList = new List<OrderProducts>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = String.Format(@"SELECT
            COUNT(IdCustomer)
            FROM OrderProducts
            WHERE IdProduct = {0};", prodId);
            cmd.Connection = _sqlConnection;

            _sqlConnection.Open();
            using (SqlDataReader dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    OrderProducts op = new OrderProducts();
                    op.CustomerCount = dataReader.GetInt32(0);

                    customersPerProductsList.Add(op);
                }
            }
            _sqlConnection.Close();

            return customersPerProductsList;
        }


        public List<CustomerOrder> GetCustomerOrders()
        {
            List<CustomerOrder> ordersList = new List<CustomerOrder>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = @"SELECT IdCustomerOrder, OrderNumber, DateCreated, IdCustomer, PaymentType, Shipping, IdPaymentOption 
                              FROM CustomerOrder";

            cmd.Connection = _sqlConnection;

            _sqlConnection.Open();
            using (SqlDataReader dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    CustomerOrder co = new CustomerOrder();
                    co.IdCustomerOrder = dataReader.GetInt32(0);
                    co.OrderNumber = dataReader.GetString(1);
                    co.DateCreated = dataReader.GetString(2);
                    co.IdCustomer = dataReader.GetInt32(3);
                    co.PaymentType = dataReader.GetString(4);
                    co.Shipping = dataReader.GetString(5);
                    co.IdPaymentOption = dataReader.GetInt32(6);

                    ordersList.Add(co);
                }
            }
            _sqlConnection.Close();

            return ordersList;
        }

        public List<PaymentOption> GetPaymentOptions(int custId)
        {
            //list of payment options corresponding to the given customer id
            List<PaymentOption> paymentOptionList = new List<PaymentOption>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            //build up list of payment options for the user
            cmd.CommandText = "SELECT IdPaymentOption, IdCustomer, Name, AccountNumber FROM PaymentOption WHERE IdCustomer = " + custId;
            cmd.Connection = _sqlConnection;

            _sqlConnection.Open();
            using (SqlDataReader dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    PaymentOption po = new PaymentOption();
                    po.IdPaymentOption = dataReader.GetInt32(0);
                    po.IdCustomer = dataReader.GetInt32(1);
                    po.Name = dataReader.GetString(2);
                    po.AccountNumber = dataReader.GetString(3);

                    paymentOptionList.Add(po);
                }
            }
            _sqlConnection.Close();

            return paymentOptionList;
        }

        private void update(string commandString)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = commandString;
            cmd.Connection = _sqlConnection;

            _sqlConnection.Open();
            cmd.ExecuteNonQuery();
            _sqlConnection.Close();
        }

    }
}
