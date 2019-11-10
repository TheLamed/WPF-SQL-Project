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
    public class ServerController
    {
        #region Propertys

        private SqlConnection connection;
        private SqlCommand getServers, 
            getInfo, 
            removeServer, 
            getLocations,
            addServer, 
            updateSerevr,
            removeLocation,
            addLocation,
            updateLocation, 
            getServer;
        private SqlDataReader reader;

        public event Action ServersChanged;
        public event Action LocationsChanged;

        #endregion

        #region Constructors
        public ServerController()
        {
            connection = new SqlConnection(AppSettings.ConnectionString);

            getServers = new SqlCommand() { Connection = connection };
            getServers.CommandText = "EXEC GetServers";

            getInfo = new SqlCommand() { Connection = connection };
            getInfo.CommandText = "EXEC GetLocation @ID";
            getInfo.Parameters.Add("@ID", SqlDbType.Int).Value = DBNull.Value;

            removeServer = new SqlCommand() { Connection = connection };
            removeServer.CommandText = "EXEC RemoveServer @ID";
            removeServer.Parameters.Add("@ID", SqlDbType.Int).Value = DBNull.Value;

            getLocations = new SqlCommand() { Connection = connection };
            getLocations.CommandText = "EXEC getLocations @part";
            getLocations.Parameters.Add("@part", SqlDbType.Text).Value = DBNull.Value;

            addServer = new SqlCommand() { Connection = connection };
            addServer.CommandText = "EXEC AddServer @proc, @RAM, @SSD, @Location";
            addServer.Parameters.Add("@proc", SqlDbType.Text).Value = DBNull.Value;
            addServer.Parameters.Add("@RAM", SqlDbType.Int).Value = DBNull.Value;
            addServer.Parameters.Add("@SSD", SqlDbType.Int).Value = DBNull.Value;
            addServer.Parameters.Add("@Location", SqlDbType.Int).Value = DBNull.Value;

            updateSerevr = new SqlCommand() { Connection = connection };
            updateSerevr.CommandText = "EXEC UpdateServer @ID, @proc, @RAM, @SSD, @Location";
            updateSerevr.Parameters.Add("@ID", SqlDbType.Int).Value = DBNull.Value;
            updateSerevr.Parameters.Add("@proc", SqlDbType.Text).Value = DBNull.Value;
            updateSerevr.Parameters.Add("@RAM", SqlDbType.Int).Value = DBNull.Value;
            updateSerevr.Parameters.Add("@SSD", SqlDbType.Int).Value = DBNull.Value;
            updateSerevr.Parameters.Add("@Location", SqlDbType.Int).Value = DBNull.Value;

            removeLocation = new SqlCommand() { Connection = connection };
            removeLocation.CommandText = "EXEC RemoveLocation @ID";
            removeLocation.Parameters.Add("@ID", SqlDbType.Int).Value = DBNull.Value;

            addLocation = new SqlCommand() { Connection = connection };
            addLocation.CommandText = "EXEC AddLocation @country, @city, @image";
            addLocation.Parameters.Add("@country", SqlDbType.Text).Value = DBNull.Value;
            addLocation.Parameters.Add("@city", SqlDbType.Text).Value = DBNull.Value;
            addLocation.Parameters.Add("@image", SqlDbType.Image).Value = DBNull.Value;

            updateLocation = new SqlCommand() { Connection = connection };
            updateLocation.CommandText = "EXEC UpdateLocation @ID, @country, @city, @image";
            updateLocation.Parameters.Add("@ID", SqlDbType.Int).Value = DBNull.Value;
            updateLocation.Parameters.Add("@country", SqlDbType.Text).Value = DBNull.Value;
            updateLocation.Parameters.Add("@city", SqlDbType.Text).Value = DBNull.Value;
            updateLocation.Parameters.Add("@image", SqlDbType.Image).Value = DBNull.Value;

            getServer = new SqlCommand() { Connection = connection };
            getServer.CommandText = "EXEC GetServer @ID";
            getServer.Parameters.Add("@ID", SqlDbType.Int).Value = DBNull.Value;
        }
        #endregion

        #region Methods

        public List<Server> GetServers()
        {
            List<Server> servers = new List<Server>();
            try
            {
                connection.Open();
                reader = getServers.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        servers.Add(new Server(reader));
            }
            finally
            {
                Close();
            }
            return servers;
        }

        public void RemoveServer(Server server)
        {
            if (server.ID == null) throw new NullIdException("Server ID is null.");

            removeServer.Parameters["@ID"].Value = server.ID;

            try
            {
                connection.Open();
                if (removeServer.ExecuteNonQuery() == 0)
                    throw new ZeroRowsExecutedException();
            }
            finally
            {
                Close();
                removeServer.Parameters["@ID"].Value = DBNull.Value;
            }
            ServersChanged?.Invoke();
        }

        public void AddServer(Server server)
        {
            if (server.LocationID == null) throw new NullIdException("Location id is null.");

            addServer.Parameters["@Location"].Value = server.LocationID;
            if (server.Processor != null) addServer.Parameters["@proc"].Value = server.Processor;
            if (server.RAM != null) addServer.Parameters["@RAM"].Value = server.RAM;
            if (server.SSD != null) addServer.Parameters["@SSD"].Value = server.SSD;

            try
            {
                connection.Open();
                if (addServer.ExecuteNonQuery() == 0)
                    throw new ZeroRowsExecutedException();
            }
            finally
            {
                Close();
                addServer.Parameters["@proc"].Value = DBNull.Value;
                addServer.Parameters["@RAM"].Value = DBNull.Value;
                addServer.Parameters["@SSD"].Value = DBNull.Value;
                addServer.Parameters["@Location"].Value = DBNull.Value;
            }
        }

        public void UpdateServer(int id, Server server)
        {
            updateSerevr.Parameters["@ID"].Value = id;
            if (server.Processor != null) updateSerevr.Parameters["@proc"].Value = server.Processor;
            if (server.RAM != null) updateSerevr.Parameters["@RAM"].Value = server.RAM;
            if (server.SSD != null) updateSerevr.Parameters["@SSD"].Value = server.SSD;
            if (server.LocationID != null) updateSerevr.Parameters["@Location"].Value = server.LocationID;

            try
            {
                connection.Open();
                if (updateSerevr.ExecuteNonQuery() == 0)
                    throw new ZeroRowsExecutedException();
            }
            finally
            {
                Close();
                updateSerevr.Parameters["@ID"].Value = DBNull.Value;
                updateSerevr.Parameters["@proc"].Value = DBNull.Value;
                updateSerevr.Parameters["@RAM"].Value = DBNull.Value;
                updateSerevr.Parameters["@SSD"].Value = DBNull.Value;
                updateSerevr.Parameters["@Location"].Value = DBNull.Value;
            }
        }

        public ServerInfo GetInfo(Server server)
        {
            if (server.LocationID == null) throw new NullIdException("Location id is null.");

            ServerInfo info = null;
            getInfo.Parameters["@ID"].Value = server.LocationID;

            try
            {
                connection.Open();
                reader = getInfo.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    info = new ServerInfo(server, reader);
                }
                else
                {
                    info = new ServerInfo(server);
                }
            }
            finally
            {
                Close();
                getInfo.Parameters["@ID"].Value = DBNull.Value;
            }
            return info;
        }

        public List<Location> GetLocations(string part = "")
        {
            if (part != null) getLocations.Parameters["@part"].Value = part;

            List<Location> locations = new List<Location>();
            try
            {
                connection.Open();
                reader = getLocations.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        locations.Add(new Location(reader));
            }
            finally
            {
                Close();
                getLocations.Parameters["@part"].Value = DBNull.Value;
            }
            return locations;
        }

        public void RemoveLocation(Location location)
        {
            if (location.ID == null) throw new NullIdException("Location id is null.");

            removeLocation.Parameters["@ID"].Value = location.ID;

            try
            {
                connection.Open();
                if (removeLocation.ExecuteNonQuery() == 0)
                    throw new ZeroRowsExecutedException();
            }
            finally
            {
                Close();
                removeLocation.Parameters["@ID"].Value = DBNull.Value;
            }
            LocationsChanged?.Invoke();
        }

        public void AddLocation(Location location)
        {
            if (location.Country == null) throw new NullReqiuredFieldException("Country");

            addLocation.Parameters["@country"].Value = location.Country;
            if(location.City != null) addLocation.Parameters["@city"].Value = location.City;
            if(location.Image != null) addLocation.Parameters["@image"].Value = location.Image;

            try
            {
                connection.Open();
                if (addLocation.ExecuteNonQuery() == 0)
                    throw new ZeroRowsExecutedException();
            }
            finally
            {
                Close();
                addLocation.Parameters["@country"].Value = DBNull.Value;
                addLocation.Parameters["@city"].Value = DBNull.Value;
                addLocation.Parameters["@image"].Value = DBNull.Value;
            }
            LocationsChanged?.Invoke();
        }

        public void UpdateLocation(Location location)
        {
            if (location.ID == null) throw new NullIdException("Location id is null.");
            if (location.Country == null) throw new NullReqiuredFieldException("Country");

            updateLocation.Parameters["@country"].Value = location.Country;
            updateLocation.Parameters["@ID"].Value = location.ID;
            if (location.City != null) updateLocation.Parameters["@city"].Value = location.City;
            if (location.Image != null) updateLocation.Parameters["@image"].Value = location.Image;

            try
            {
                connection.Open();
                if (updateLocation.ExecuteNonQuery() == 0)
                    throw new ZeroRowsExecutedException();
            }
            finally
            {
                Close();
                updateLocation.Parameters["@country"].Value = DBNull.Value;
                updateLocation.Parameters["@ID"].Value = DBNull.Value;
                updateLocation.Parameters["@city"].Value = DBNull.Value;
                updateLocation.Parameters["@image"].Value = DBNull.Value;
            }
            LocationsChanged?.Invoke();
        }

        public Server GetServer(int id)
        {
            Server server = null;

            getServer.Parameters["@ID"].Value = id;

            try
            {
                connection.Open();
                reader = getServer.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    server = new Server(reader);
                }
            }
            finally
            {
                Close();
                getServer.Parameters["@ID"].Value = DBNull.Value;
            }
            return server;
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
