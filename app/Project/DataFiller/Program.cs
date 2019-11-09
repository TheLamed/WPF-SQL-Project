using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFiller
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Hosting;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            SqlCommand command = new SqlCommand() { Connection = connection };
            command.CommandText = "UPDATE Location SET Photo = @p WHERE ID = 1";
            command.Parameters.Add("@p", SqlDbType.Image);

            byte[] img;
            FileStream f = new FileStream(@"C:\Users\PC\Desktop\datacenter-840x420.jpg", FileMode.Open);
            using (MemoryStream m = new MemoryStream())
            {
                f.CopyTo(m);
                img = m.ToArray();
            }
            command.Parameters["@p"].Value = img;
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
