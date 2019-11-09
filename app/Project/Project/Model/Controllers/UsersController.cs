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
    class UsersController
    {
        #region Propertys

        private SqlConnection connection;
        private SqlCommand getUsers, 
            getInfo, 
            removeUser, 
            addUser, 
            updateUser, 
            isLoginUsed, 
            isAdmin, 
            setAdmin;
        private SqlDataReader reader;

        public event Action UsersChanged;

        #endregion

        public UsersController()
        {
            connection = new SqlConnection(AppSettings.ConnectionString);

            getUsers = new SqlCommand() { Connection = connection };
            getUsers.CommandText = "EXEC GetUsers @login";
            getUsers.Parameters.Add("@login", SqlDbType.Text).Value = DBNull.Value;

            getInfo = new SqlCommand() { Connection = connection };
            getInfo.CommandText = "EXEC GetOrders @ID";
            getInfo.Parameters.Add("@ID", SqlDbType.Int).Value = DBNull.Value;

            removeUser = new SqlCommand() { Connection = connection };
            removeUser.CommandText = "EXEC RemoveUser @ID";
            removeUser.Parameters.Add("@ID", SqlDbType.Int).Value = DBNull.Value;

            addUser = new SqlCommand() { Connection = connection };
            addUser.CommandText = "EXEC RegisterUser @name, @surname, @login, @password, @email, @isAdmin";
            addUser.Parameters.Add("@name", SqlDbType.Text).Value = DBNull.Value;
            addUser.Parameters.Add("@surname", SqlDbType.Text).Value = DBNull.Value;
            addUser.Parameters.Add("@login", SqlDbType.Text).Value = DBNull.Value;
            addUser.Parameters.Add("@password", SqlDbType.Text).Value = DBNull.Value;
            addUser.Parameters.Add("@email", SqlDbType.Text).Value = DBNull.Value;
            addUser.Parameters.Add("@isAdmin", SqlDbType.Int).Value = DBNull.Value;

            updateUser = new SqlCommand() { Connection = connection };
            updateUser.CommandText = "EXEC UpadeUser @ID, @name, @surname, @login, @password, @email";
            updateUser.Parameters.Add("@ID", SqlDbType.Int).Value = DBNull.Value;
            updateUser.Parameters.Add("@name", SqlDbType.Text).Value = DBNull.Value;
            updateUser.Parameters.Add("@surname", SqlDbType.Text).Value = DBNull.Value;
            updateUser.Parameters.Add("@login", SqlDbType.Text).Value = DBNull.Value;
            updateUser.Parameters.Add("@password", SqlDbType.Text).Value = DBNull.Value;
            updateUser.Parameters.Add("@email", SqlDbType.Text).Value = DBNull.Value;

            isLoginUsed = new SqlCommand() { Connection = connection };
            isLoginUsed.CommandText = "EXEC IsLoginUsed @login";
            isLoginUsed.Parameters.Add("@login", SqlDbType.Text).Value = DBNull.Value;

            isAdmin = new SqlCommand() { Connection = connection };
            isAdmin.CommandText = "EXEC IsAdmin @id";
            isAdmin.Parameters.Add("@id", SqlDbType.Int).Value = DBNull.Value;

            setAdmin = new SqlCommand() { Connection = connection };
            setAdmin.CommandText = "EXEC SetAdmin @isAdmin, @ID";
            setAdmin.Parameters.Add("@isAdmin", SqlDbType.Int).Value = DBNull.Value;
            setAdmin.Parameters.Add("@ID", SqlDbType.Int).Value = DBNull.Value;
        }

        #region Methods

        public List<User> GetUsers(string login = "")
        {
            List<User> users = new List<User>();

            if (login == null) login = "";
            getUsers.Parameters["@login"].Value = login;

            try
            {
                connection.Open();
                reader = getUsers.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        users.Add(new User(reader));
            }
            finally
            {
                Close();
                getUsers.Parameters["@login"].Value = DBNull.Value;
            }
            return users;
        }

        public UserInfo GetInfo(User user)
        {
            UserInfo info = null;
            getInfo.Parameters["@ID"].Value = user.ID;

            bool isAdmin = IsAdmin(user);

            try
            {
                connection.Open();
                reader = getInfo.ExecuteReader();
                if (reader.HasRows)
                {
                    info = new UserInfo(user, reader, isAdmin);
                }
                else
                {
                    info = new UserInfo(user, isAdmin);
                }
            }
            finally
            {
                Close();
                getInfo.Parameters["@ID"].Value = DBNull.Value;
            }
            return info;
        }

        public void RemoveUser(User user)
        {
            if (user.ID == null) throw new NullIdException("User ID is null.");

            removeUser.Parameters["@ID"].Value = user.ID;

            try
            {
                connection.Open();
                if (removeUser.ExecuteNonQuery() == 0)
                    throw new ZeroRowsExecutedException();
            }
            finally
            {
                Close();
                removeUser.Parameters["@ID"].Value = DBNull.Value;
            }
            UsersChanged?.Invoke();
        }

        public void AddUser(User user, bool isAdmin)
        {
            if (user.Login == null || user.Login == "")
                throw new NullReqiuredFieldException("Login");
            if (user.Password == null || user.Password == "")
                throw new NullReqiuredFieldException("Password");

            addUser.Parameters["@login"].Value = user.Login;
            addUser.Parameters["@password"].Value = user.Password;
            addUser.Parameters["@isAdmin"].Value = isAdmin ? 1 : 0;
            if (user.Name != null) addUser.Parameters["@name"].Value = user.Name;
            if (user.Surname != null) addUser.Parameters["@surname"].Value = user.Surname;
            if (user.Email != null) addUser.Parameters["@email"].Value = user.Email;

            try
            {
                connection.Open();
                if (addUser.ExecuteNonQuery() <= 0)
                    throw new ZeroRowsExecutedException();
            }
            finally
            {
                Close();
                addUser.Parameters["@name"].Value = DBNull.Value;
                addUser.Parameters["@surname"].Value = DBNull.Value;
                addUser.Parameters["@login"].Value = DBNull.Value;
                addUser.Parameters["@password"].Value = DBNull.Value;
                addUser.Parameters["@email"].Value = DBNull.Value;
            }
        }

        public void UpdateUser(User user, bool isAdmin)
        {
            if (user.ID == null) throw new NullIdException("User id is null.");
            if (user.Login == null) throw new NullReqiuredFieldException("Login");
            if (user.Password == null) throw new NullReqiuredFieldException("Password");

            updateUser.Parameters["@ID"].Value = user.ID;
            updateUser.Parameters["@login"].Value = user.Login;
            updateUser.Parameters["@password"].Value = user.Password;

            if (user.Name != null) updateUser.Parameters["@name"].Value = user.Name;
            if (user.Surname != null) updateUser.Parameters["@surname"].Value = user.Surname;
            if (user.Email != null) updateUser.Parameters["@email"].Value = user.Email;

            try
            {
                connection.Open();
                if (updateUser.ExecuteNonQuery() == 0)
                    throw new ZeroRowsExecutedException();
            }
            finally
            {
                Close();
                updateUser.Parameters["@ID"].Value = DBNull.Value;
                updateUser.Parameters["@login"].Value = DBNull.Value;
                updateUser.Parameters["@password"].Value = DBNull.Value;
                updateUser.Parameters["@name"].Value = DBNull.Value;
                updateUser.Parameters["@surname"].Value = DBNull.Value;
                updateUser.Parameters["@email"].Value = DBNull.Value;
            }
            SetAdmin(isAdmin, user);
        }

        public bool IsLoginUsed(string login)
        {
            if (login == null || login == "")
                throw new NullReqiuredFieldException("Login");

            isLoginUsed.Parameters["@login"].Value = login;

            try
            {
                connection.Open();
                reader = isLoginUsed.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0) != 0 ? true : false;
                }
            }
            finally
            {
                Close();
                isLoginUsed.Parameters["@login"].Value = DBNull.Value;
            }
            return false;
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

        public void SetAdmin(bool isAdmin, User user)
        {
            if (user.ID == null) throw new NullIdException("User id is null.");

            setAdmin.Parameters["@ID"].Value = user.ID;
            setAdmin.Parameters["@isAdmin"].Value = isAdmin ? 1 : 0;

            try
            {
                connection.Open();
                if (setAdmin.ExecuteNonQuery() == 0)
                    throw new ZeroRowsExecutedException();
            }
            finally
            {
                Close();
                setAdmin.Parameters["@isAdmin"].Value = DBNull.Value;
                setAdmin.Parameters["@ID"].Value = DBNull.Value;
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
