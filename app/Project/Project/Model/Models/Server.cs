using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Models
{
    public class Server
    {
        public int? ID { get; private set; }
        public string Processor { get; private set; }
        public int? RAM { get; private set; }
        public int? SSD { get; private set; }
        public int? LocationID { get; private set; }

        public Server(SqlDataReader reader)
        {
            ID = reader.GetInt32(0);
            Processor = reader.IsDBNull(1) ? null : reader.GetString(1);
            if (reader.IsDBNull(2)) RAM = null;
            else RAM = reader.GetInt32(2);
            if (reader.IsDBNull(3)) SSD = null;
            else SSD = reader.GetInt32(3);
            if (reader.IsDBNull(4)) LocationID = null;
            else LocationID = reader.GetInt32(4);
        }

        public Server(string processor = null, int? RAM = null, int? SSD = null, int? locationID = null)
        {
            Processor = processor;
            this.RAM = RAM;
            this.SSD = SSD;
            LocationID = locationID;
        }
        protected Server(Server server)
        {
            ID = server.ID;
            Processor = server.Processor;
            RAM = server.RAM;
            SSD = server.SSD;
            LocationID = server.LocationID;
        }

        public override string ToString()
        {
            string str = $"{(ID != null ? ID.ToString() : "_")}:";

            if (Processor != null)
                str += " Processor: " + Processor;
            if (RAM != null)
                str += " RAM: " + RAM;
            if (SSD != null)
                str += " SSD: " + SSD;

            return str;
        }
    }
}
