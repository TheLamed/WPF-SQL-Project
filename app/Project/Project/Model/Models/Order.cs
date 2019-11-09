using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Models
{
    public class Order
    {
        public int ID { get; private set; }
        public int OwnerID { get; private set; }
        public int ServerID { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime FinishDate { get; private set; }

        public Order(int ID, int ownerID, int serverID, DateTime startDate, DateTime finishDate)
        {
            this.ID = ID;
            OwnerID = ownerID;
            ServerID = serverID;
            StartDate = startDate;
            FinishDate = finishDate;
        }

        protected Order(Order order)
        {
            ID = order.ID;
            OwnerID = order.OwnerID;
            ServerID = order.ServerID;
            StartDate = order.StartDate;
            FinishDate = order.FinishDate;
        }

        public Order(SqlDataReader reader)
        {
            ID = reader.GetInt32(0);
            OwnerID = reader.GetInt32(1);
            ServerID = reader.GetInt32(2);
            StartDate = reader.GetDateTime(3);
            FinishDate = reader.GetDateTime(4);
        }

        public override string ToString() 
            => $"{ID}: Owner {OwnerID}, Server {ServerID}";
    }
}
