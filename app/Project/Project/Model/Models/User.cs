using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Models
{
    public class User
    {
        public int? ID { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Email { get; private set; }

        public User()
        {

        }

        public User(SqlDataReader reader)
        {
            ID = reader.GetInt32(0);
            Login = reader.GetString(3);
            Password = reader.GetString(4);
            Name = reader.IsDBNull(1) ? null : reader.GetString(1);
            Surname = reader.IsDBNull(2) ? null : reader.GetString(2);
            Email = reader.IsDBNull(5) ? null : reader.GetString(5);
        }

        public User(string login, string password, string name = null, string surname = null, string email = null, int? id = null)
        {
            ID = id;
            Login = login;
            Password = password;
            Name = name;
            Surname = surname;
            Email = email;
        }

        protected User(User user)
        {
            ID = user.ID;
            Login = user.Login;
            Password = user.Password;
            Name = user.Name;
            Surname = user.Surname;
            Email = user.Email;
        }

        public override string ToString() 
            => $"{(ID != null ? ID.ToString() : "_")}: {Login}";
    }
}
