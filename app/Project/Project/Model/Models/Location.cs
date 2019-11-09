using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Models
{
    public class Location
    {
        public int? ID { get; private set; }
        public string Country { get; private set; }
        public string City { get; private set; }
        public byte[] Image { get; private set; }

        public string toString => ToString();

        public Location(int? id = null, string Country = null, string city = null, byte[] image = null)
        {
            ID = id;
            this.Country = Country;
            City = city;
            Image = image;
        }
        public Location(SqlDataReader reader)
        {
            ID = reader.GetInt32(0);
            Country = reader.IsDBNull(1) ? null : reader.GetString(1);
            City = reader.IsDBNull(2) ? null : reader.GetString(2);
            Image = reader.IsDBNull(3) ? null : reader.GetValue(3) as byte[];
        }

        public override string ToString()
            => $"{Country}" + (City != null ? $", {City}" : "");
    }
}
