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
    public class UserController
    {
        #region Propertys

        private SqlConnection connection;
        private SqlCommand getUser, isAdmin, registerUser;
        private SqlDataReader reader;

        #endregion

        public UserController()
        {
            connection = new SqlConnection(AppSettings.ConnectionString);

            getUser = new SqlCommand() { Connection = connection };
            getUser.CommandText = "EXEC GetUser @login, @password";
            getUser.Parameters.Add("@login", SqlDbType.Text).Value = DBNull.Value;
            getUser.Parameters.Add("@password", SqlDbType.Text).Value = DBNull.Value;

            isAdmin = new SqlCommand() { Connection = connection };
            isAdmin.CommandText = "EXEC IsAdmin @id";
            isAdmin.Parameters.Add("@id", SqlDbType.Int).Value = DBNull.Value;

            registerUser = new SqlCommand() { Connection = connection };
            registerUser.CommandText = "EXEC RegisterUser @name, @surname, @login, @password, @email";
            registerUser.Parameters.Add("@name", SqlDbType.Text).Value = DBNull.Value;
            registerUser.Parameters.Add("@surname", SqlDbType.Text).Value = DBNull.Value;
            registerUser.Parameters.Add("@login", SqlDbType.Text).Value = DBNull.Value;
            registerUser.Parameters.Add("@password", SqlDbType.Text).Value = DBNull.Value;
            registerUser.Parameters.Add("@email", SqlDbType.Text).Value = DBNull.Value;

        }

        #region Methods

        public User Login(string login, string password)
        {
            if (login == null) throw new NullReqiuredFieldException("Login");
            if (password == null) throw new NullReqiuredFieldException("Password");

            getUser.Parameters["@login"].Value = login;
            getUser.Parameters["@password"].Value = password;

            try
            {
                connection.Open();
                reader = getUser.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    return new User(reader);
                }
            }
            finally
            {
                Close();
                getUser.Parameters["@login"].Value = DBNull.Value;
                getUser.Parameters["@password"].Value = DBNull.Value;
            }
            return null;
        }

        public bool IsAdmin(User user)
        {
            if (user.ID == null) throw new NullIdException("User id is null.");

            isAdmin.Parameters["@id"].Value = user.ID;

            try
            {
                connection.Open();
                reader = isAdmin.ExecuteReader();
                if (reader.HasRows) return true;
            }
            finally
            {
                Close();
                isAdmin.Parameters["@id"].Value = DBNull.Value;
            }
            return false;
        }

        public void RegisterUser(User user)
        {
            if (user.Login == null || user.Login == "")
                throw new NullReqiuredFieldException("Login");
            if (user.Password == null || user.Password == "")
                throw new NullReqiuredFieldException("Password");

            registerUser.Parameters["@login"].Value = user.Login;
            registerUser.Parameters["@password"].Value = user.Password;
            if (user.Name != null) registerUser.Parameters["@name"].Value = user.Name;
            if (user.Surname != null) registerUser.Parameters["@surname"].Value = user.Surname;
            if (user.Email != null) registerUser.Parameters["@email"].Value = user.Email;

            try
            {
                connection.Open();
                if (registerUser.ExecuteNonQuery() <= 0)
                    throw new ZeroRowsExecutedException();
            }
            finally
            {
                Close();
                registerUser.Parameters["@name"].Value = DBNull.Value;
                registerUser.Parameters["@surname"].Value = DBNull.Value;
                registerUser.Parameters["@login"].Value = DBNull.Value;
                registerUser.Parameters["@password"].Value = DBNull.Value;
                registerUser.Parameters["@email"].Value = DBNull.Value;
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
