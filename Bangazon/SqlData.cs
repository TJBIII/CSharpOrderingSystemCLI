using System;
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
           
        }

        public void CreatePaymentOption(PaymentOption po)
        {
            
        }

        public int GetNextPaymentOptionId()
        {
            return 0;
        }

        public void CreateOrderProduct(OrderProducts op)
        {
            
        }

        public void CreateCustomerOrder(CustomerOrder co)
        {
            
        }

        public List<Customer> GetCustomers()
        { 
           
        }

        public List<Product> GetProducts()
        {
           
        }

        public List<Product> GetSingleProduct(int id)
        {
            
        }

        public List<OrderProducts> GetOrderProducts()
        {
           
            
        }

        public List<OrderProducts> GetOrderProductsCount()
        {
           
        }

        public List<OrderProducts> GetCustomersPerProduct(int prodId)
        {
           
        }


        public List<CustomerOrder> GetCustomerOrders()
        {

        }

        public List<PaymentOption> GetPaymentOptions(int custId)
        { 

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
