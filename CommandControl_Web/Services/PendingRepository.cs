using CommandControl_Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace CommandControl_Web.Services
{
    public class PendingRepository
    {
        public Pending[] GetPendings()
        {
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = Settings.connstring
            };
            IList<Pending> pending = new List<Pending>();
            sqlConnection.Open();
            SqlCommand comm = new SqlCommand("select * from Pending", sqlConnection);
            SqlDataReader dr = comm.ExecuteReader();

            while (dr.Read())
            {
                Pending s = new Pending();
                s.Id = dr.GetInt32(0);
                s.ClientId = dr.GetInt32(1);
                s.CommandId = dr.GetInt32(2);
                s.QueuedDT = dr.GetDateTime(3);
                if (!dr.IsDBNull(4))
                    s.SentDT = dr.GetDateTime(4);
                pending.Add(s);
            };

            return pending.ToArray<Pending>();
        }

        public Pending[] GetPending(int id)
        {
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = Settings.connstring
            };
            IList<Pending> pending = new List<Pending>();
            sqlConnection.Open();
            SqlCommand comm = new SqlCommand("select * from Pending where ClientId = @clientId and SentDT is null", sqlConnection);
            comm.Parameters.Add(new SqlParameter("clientid", id));
            SqlDataReader dr = comm.ExecuteReader();

            while (dr.Read())
            {
                Pending s = new Pending();
                s.Id = dr.GetInt32(0);
                s.ClientId = dr.GetInt32(1);
                s.CommandId = dr.GetInt32(2);
                s.QueuedDT = dr.GetDateTime(3);
                if (!dr.IsDBNull(4))
                    s.SentDT = dr.GetDateTime(4);
                pending.Add(s);
            };

            return pending.ToArray<Pending>();
        }

        public bool NewPending(Pending pending)
        {
            // save
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = Settings.connstring
            };

            sqlConnection.Open();
            SqlCommand comm = new SqlCommand("insert into Pending (ClientId, CommandId, QueuedDT) values (@clientId, @commandId, @queuedDT)");
            comm.Parameters.Add(new SqlParameter("@clientId", pending.ClientId));
            comm.Parameters.Add(new SqlParameter("@commandId", pending.CommandId));
            comm.Parameters.Add(new SqlParameter("@queuedDT", DateTime.Now));

            comm.Connection = sqlConnection;

            int result = comm.ExecuteNonQuery();
            if (result > 0)
                return true;
            else
                return false;

        }

        public bool UpdatePending(int id)
        {
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = Settings.connstring
            };

            sqlConnection.Open();
            SqlCommand comm = new SqlCommand("update Pending set SentDT = @sentdt where id = @id");
            comm.Parameters.Add(new SqlParameter("@sentdt", DateTime.Now));
            comm.Parameters.Add(new SqlParameter("@id", id));

            comm.Connection = sqlConnection;

            int result = comm.ExecuteNonQuery();
            if (result > 0)
                return true;
            else
                return false;
        }
    }
}