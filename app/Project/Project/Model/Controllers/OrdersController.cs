using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Controllers
{
    public class OrdersController
    {
        #region Propertys

        private SqlConnection connection;
        private SqlCommand getOrders, getUserById, getUsersByOrder;
        private SqlDataReader reader;

        public event Action OrdersChanged;

        #endregion

        public OrdersController()
        {
            connection = new SqlConnection(AppSettings.ConnectionString);

            getOrders = new SqlCommand() { Connection = connection };
            getOrders.CommandText = "EXEC GetAllOrders";

            getUserById = new SqlCommand() { Connection = connection };
            getUserById.CommandText = "EXEC GetUserById @ID";
            getUserById.Parameters.Add("@ID", SqlDbType.Int).Value = DBNull.Value;

            getUsersByOrder = new SqlCommand() { Connection = connection };
            getUsersByOrder.CommandText = "EXEC GetUsersByOrder @ID";
            getUsersByOrder.Parameters.Add("@ID", SqlDbType.Int).Value = DBNull.Value;
        }

        #region Methods

        public List<Order> GetOrders()
        {
            List<Order> orders = new List<Order>();
            try
            {
                connection.Open();
                reader = getOrders.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        orders.Add(new Order(reader));
            }
            finally
            {
                Close();
            }
            return orders;
        }

        public OrderInfo GetInfo(Order order)
        {
            OrderInfo info = null;
            getUserById.Parameters["@ID"].Value = order.OwnerID;
            getUsersByOrder.Parameters["@ID"].Value = order.ID;

            string owner;
            List<string> users = new List<string>();

            try
            {
                connection.Open();
                reader = getUserById.ExecuteReader();
                reader.Read();
                owner = new User(reader).Login;
            }
            finally
            {
                Close();
                getUserById.Parameters["@ID"].Value = DBNull.Value;
            }

            try
            {
                connection.Open();
                reader = getUsersByOrder.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        users.Add(new User(reader).Login);
            }
            finally
            {
                Close();
                getUsersByOrder.Parameters["@ID"].Value = DBNull.Value;
            }

            info = new OrderInfo(order, owner, users);

            return info;
        }



        private void Close()
        {
            if (!(reader?.IsClosed ?? true))
                reader.Close();
            connection.Close();
        }
        #endregion
    }
}
