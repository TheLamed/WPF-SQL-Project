using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Models
{
    public class ServerInfo : Server
    {
        public string Country { get; private set; }
        public string City { get; private set; }
        public byte[] Image { get; private set; }

        public ServerInfo(Server server, string Country = null, string city = null, byte[] image = null) : base(server)
        {
            this.Country = Country;
            City = city;
            Image = image;
        }
        public ServerInfo(Server server, SqlDataReader reader) : base(server)
        {
            Country = reader.IsDBNull(0) ? null : reader.GetString(0);
            City = reader.IsDBNull(1) ? null : reader.GetString(1);
            Image = reader.IsDBNull(2) ? null : reader.GetValue(2) as byte[];
        }

        public override string ToString() 
            => $"ID: {ID}\nProcessor: {Processor}\nRAM: {RAM}\nSSD: {SSD}\nCountry: {Country}\nCity: {City}";
    }
}
