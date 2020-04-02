using CommandControl_Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CommandControl_Web.Services
{
    public class ClientRepository
    {
        public Client[] GetClients()
        {
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = Settings.connstring
            };
            IList<Client> clients = new List<Client>();
            sqlConnection.Open();
            SqlCommand comm = new SqlCommand("select * from Clients", sqlConnection);
            SqlDataReader dr = comm.ExecuteReader();

            while (dr.Read())
            {
                Client s = new Client();
                s.Id = dr.GetInt32(0);
                s.ClientName = dr.GetString(1);
                s.LastAccessed = dr.GetDateTime(2);
                s.FirstSeen = dr.GetDateTime(3);
                clients.Add(s);
            };

            return clients.ToArray<Client>();
        }

        public Client GetClient(int id)
        {
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = Settings.connstring
            };
            sqlConnection.Open();
            
            SqlCommand comm = new SqlCommand("select * from Clients where id = " + id.ToString(), sqlConnection);
            SqlDataReader dr = comm.ExecuteReader();
            Client ret_val = new Client();

            if (dr.Read())
            {
                ret_val.Id = dr.GetInt32(0);
                ret_val.ClientName = dr.GetString(1);
                ret_val.LastAccessed = dr.GetDateTime(2);
                ret_val.FirstSeen = dr.GetDateTime(3);
            };

            return ret_val;
        }

        public Client ClientKnown(string clientName)
        {
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = Settings.connstring
            };
            sqlConnection.Open();

            SqlCommand comm = new SqlCommand("select * from Clients where ClientName = @clientName", sqlConnection);
            comm.Parameters.Add(new SqlParameter("@clientName", clientName));
            SqlDataReader dr = comm.ExecuteReader();
            Client ret_val = new Client();
            ret_val.Id = -1;

            if (dr.Read())
            {
                ret_val.Id = dr.GetInt32(0);
            };

            return ret_val;
        }

        public bool NewClient(Client client)
        {
            // validate
            if (client.FirstSeen == DateTime.MinValue)
                client.FirstSeen = DateTime.Now;

            if (client.LastAccessed== DateTime.MinValue)
                client.LastAccessed = DateTime.Now;

            // save
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = Settings.connstring
            };

            sqlConnection.Open();
            SqlCommand comm = new SqlCommand("insert into Clients (ClientName, LastAccessed, FirstSeen) values (@clientName, @lastAccessed, @firstSeen)");
            comm.Parameters.Add(new SqlParameter("@clientName", client.ClientName));
            comm.Parameters.Add(new SqlParameter("@lastAccessed", client.LastAccessed));
            comm.Parameters.Add(new SqlParameter("firstSeen", client.FirstSeen));
            comm.Connection = sqlConnection;

            int result = comm.ExecuteNonQuery();
            if (result > 0)
                return true;
            else
                return false;
        }

        public bool UpdateClient(Client client)
        {
            // validate
            if (client.FirstSeen == DateTime.MinValue)
                client.FirstSeen = DateTime.Now;

            if (client.LastAccessed == DateTime.MinValue)
                client.LastAccessed = DateTime.Now;

            // save
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = Settings.connstring
            };

            sqlConnection.Open();
            SqlCommand comm = new SqlCommand("update Clients set LastAccessed = @LastAccessed where id = @id");
            comm.Parameters.Add(new SqlParameter("@lastAccessed", client.LastAccessed));
            comm.Parameters.Add(new SqlParameter("@Id", client.Id));
            comm.Connection = sqlConnection;

            int result = comm.ExecuteNonQuery();
            if (result > 0)
                return true;
            else
                return false;
        }
    }
}