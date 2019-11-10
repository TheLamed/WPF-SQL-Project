using Project.Model.Exceptions;
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
        private SqlCommand getOrders, 
            getUserById, 
            getUsersByOrder, 
            getOrderById, 
            addOrder, 
            removeUserFromOrder, 
            addUserToOrder;
        private SqlDataReader reader;

        public event Action OrdersChanged;

        #endregion

        #region Constructors
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

            getOrderById = new SqlCommand() { Connection = connection };
            getOrderById.CommandText = "EXEC GetOrderById @ID";
            getOrderById.Parameters.Add("@ID", SqlDbType.Int).Value = DBNull.Value;

            addOrder = new SqlCommand() { Connection = connection };
            addOrder.CommandText = "EXEC AddOrder @owner, @server, @start, @finish";
            addOrder.Parameters.Add("@owner", SqlDbType.Int).Value = DBNull.Value;
            addOrder.Parameters.Add("@server", SqlDbType.Int).Value = DBNull.Value;
            addOrder.Parameters.Add("@start", SqlDbType.Date).Value = DBNull.Value;
            addOrder.Parameters.Add("@finish", SqlDbType.Date).Value = DBNull.Value;

            removeUserFromOrder = new SqlCommand() { Connection = connection };
            removeUserFromOrder.CommandText = "EXEC RemoveUserFromOrder @order, @user";
            removeUserFromOrder.Parameters.Add("@order", SqlDbType.Int).Value = DBNull.Value;
            removeUserFromOrder.Parameters.Add("@user", SqlDbType.Int).Value = DBNull.Value;

            addUserToOrder = new SqlCommand() { Connection = connection };
            addUserToOrder.CommandText = "EXEC AddUserToOrder @order, @user";
            addUserToOrder.Parameters.Add("@order", SqlDbType.Int).Value = DBNull.Value;
            addUserToOrder.Parameters.Add("@user", SqlDbType.Int).Value = DBNull.Value;
        }
        #endregion

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

        public List<Order> GetOrders(IEnumerable<int> orders)
        {
            List<Order> Orders = new List<Order>();

            try
            {
                connection.Open();
                foreach (var item in orders)
                {
                    getOrderById.Parameters["@ID"].Value = item;
                    reader = getOrderById.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        Orders.Add(new Order(reader));
                        reader.Close();
                    }
                }
            }
            finally
            {
                Close();
                getOrderById.Parameters["@ID"].Value = DBNull.Value;
            }
            return Orders;
        }

        public List<User> GetUsersByOrder(Order order)
        {
            List<User> users = new List<User>();

            getUsersByOrder.Parameters["@ID"].Value = order.ID;

            try
            {
                connection.Open();
                reader = getUsersByOrder.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        users.Add(new User(reader));
            }
            finally
            {
                Close();
                getUsersByOrder.Parameters["@ID"].Value = DBNull.Value;
            }
            return users;
        }

        public void AddOrder(Order order)
        {
            addOrder.Parameters["@owner"].Value = order.OwnerID;
            addOrder.Parameters["@server"].Value = order.ServerID;
            addOrder.Parameters["@start"].Value = order.StartDate;
            addOrder.Parameters["@finish"].Value = order.FinishDate;

            try
            {
                connection.Open();
                if (addOrder.ExecuteNonQuery() == 0)
                    throw new ZeroRowsExecutedException();
            }
            finally
            {
                Close();
                addOrder.Parameters["@owner"].Value = DBNull.Value;
                addOrder.Parameters["@server"].Value = DBNull.Value;
                addOrder.Parameters["@start"].Value = DBNull.Value;
                addOrder.Parameters["@finish"].Value = DBNull.Value;
            }
        }

        public Order GetOrderById(int id)
        {
            Order order = null;

            getOrderById.Parameters["@ID"].Value = id;

            try
            {
                connection.Open();
                reader = getOrderById.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    order = new Order(reader);
                }
            }
            finally
            {
                Close();
                getOrderById.Parameters["@ID"].Value = DBNull.Value;
            }
            return order;
        }

        public void RemoveUserFromOrder(Order order, User user)
        {
            if (order == null || user == null)
                throw new NullReferenceException();

            removeUserFromOrder.Parameters["@order"].Value = order.ID;
            removeUserFromOrder.Parameters["@user"].Value = user.ID;

            try
            {
                connection.Open();
                if (removeUserFromOrder.ExecuteNonQuery() == 0)
                    throw new ZeroRowsExecutedException();
            }
            finally
            {
                Close();
                removeUserFromOrder.Parameters["@order"].Value = DBNull.Value;
                removeUserFromOrder.Parameters["@user"].Value = DBNull.Value;
            }
        }

        public void AddUserToOrder(Order order, User user)
        {
            if (order == null || user == null)
                throw new NullReferenceException();

            addUserToOrder.Parameters["@order"].Value = order.ID;
            addUserToOrder.Parameters["@user"].Value = user.ID;

            try
            {
                connection.Open();
                if (addUserToOrder.ExecuteNonQuery() == 0)
                    throw new ZeroRowsExecutedException();
            }
            finally
            {
                Close();
                addUserToOrder.Parameters["@order"].Value = DBNull.Value;
                addUserToOrder.Parameters["@user"].Value = DBNull.Value;
            }
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
