using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Models
{
    public class OrderInfo : Order
    {
        public string Owner { get; private set; }
        public List<string> Users { get; private set; }

        public OrderInfo(Order order) : base(order)
        {
            Users = new List<string>();
        }

        public OrderInfo(Order order, string owner, IEnumerable<string> users) : base(order)
        {
            Owner = owner;
            Users = new List<string>(users);
        }

        public override string ToString()
            => $"ID: {ID}\nOwner: {OwnerID} - {Owner}\nServer: {ServerID}\n" +
               $"Start  date: {StartDate.ToString("dd/MM/yyyy")}\n" +
               $"Finish date: {FinishDate.ToString("dd/MM/yyyy")}";
    }
}
