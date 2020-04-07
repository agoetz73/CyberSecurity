using CommandControl_Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace CommandControl_Web.Services
{
    public class ExecutionRepository
    {
        public Execution[] GetExecutions()
        {
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = Settings.connstring
            };
            IList<Execution> execution = new List<Execution>();
            sqlConnection.Open();
            SqlCommand comm = new SqlCommand("select * from Executions", sqlConnection);
            SqlDataReader dr = comm.ExecuteReader();

            while (dr.Read())
            {
                Execution s = new Execution();
                s.Id = dr.GetInt32(0);
                s.ClientId = dr.GetInt32(1);
                s.CommandId = dr.GetInt32(2);
                s.Result = dr.GetString(3);
                if (s.Result.IndexOf("ry98jkcdkh") > -1)
                    s.Result = s.Result.Replace("ry98jkcdkh", "********");
                s.SaveTime = dr.GetDateTime(4);
                execution.Add(s);
            };

            return execution.ToArray<Execution>();
        }

        public Execution GetExecution(int id)
        {
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = Settings.connstring
            };

            sqlConnection.Open();
            SqlCommand comm = new SqlCommand("select * from Executions where id = " + id.ToString(), sqlConnection);
            SqlDataReader dr = comm.ExecuteReader();
            Execution ret_val = new Execution();

            if (dr.Read())
            {
                ret_val.Id = dr.GetInt32(0);
                ret_val.ClientId = dr.GetInt32(1);
                ret_val.CommandId = dr.GetInt32(2);
                ret_val.Result = FormatLines(dr.GetString(3));
                ret_val.SaveTime = dr.GetDateTime(4);
            };

            return ret_val;
        }

        private string FormatLines(string s)
        {
            return s.Replace("||", "\\n");
        }

        public bool NewExecution(Execution execution)
        {
            // save
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = Settings.connstring
            };

            sqlConnection.Open();
            SqlCommand comm = new SqlCommand("insert into Executions (ClientId, CommandId, Result, SaveTime) values (@clientId, @commandId, @result, @saveTime)");
            comm.Parameters.Add(new SqlParameter("@clientId", execution.ClientId));
            comm.Parameters.Add(new SqlParameter("@commandId", execution.CommandId));
            comm.Parameters.Add(new SqlParameter("@result", execution.Result));
            comm.Parameters.Add(new SqlParameter("@saveTime", DateTime.Now));
            comm.Connection = sqlConnection;

            int result = comm.ExecuteNonQuery();
            if (result > 0)
                return true;
            else
                return false;

        }
    }
}