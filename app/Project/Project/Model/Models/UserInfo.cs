using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Models
{
    public class UserInfo : User
    {
        public List<int> Orders { get; private set; }
        public bool IsAdmin { get; private set; }

        public UserInfo(User user, bool isAdmin = false) : base(user)
        {
            Orders = new List<int>();
            IsAdmin = isAdmin;
        }
        public UserInfo(User user, IEnumerable<int> orders) : base(user)
        {
            Orders = new List<int>(orders);
        }
        public UserInfo(User user, SqlDataReader reader, bool isAdmin) : base(user)
        {
            Orders = new List<int>();
            IsAdmin = isAdmin;

            if (reader.HasRows)
                while (reader.Read())
                    Orders.Add(reader.GetInt32(0));
        }

        public override string ToString() 
            => $"ID: {ID} {(IsAdmin ? "ADMIN" : "")}\nLogin: {Login}\nFull Name: {Name} {Surname}\nEmail: {Email}\nOrders: {string.Join(", ", Orders)}";
    }
}
