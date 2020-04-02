using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommandControl_Web.Models;
using System.Data;
using System.Data.SqlClient;

namespace CommandControl_Web.Services
{
    public class CommandRepository
    {
        
        public Command[] GetScriptsFromDB()
        {
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = Settings.connstring
            };
            IList<Command> scripts = new List<Command>();
            sqlConnection.Open();
            SqlCommand comm = new SqlCommand("select * from Commands", sqlConnection);
            SqlDataReader dr = comm.ExecuteReader();
                
            while (dr.Read())
            {
                Command s = new Command();
                s.Id = dr.GetInt32(0);
                s.scriptName = dr.GetString(1);
                s.scriptType = dr.GetString(2);
                s.scriptData = dr.GetString(3);
                if (!dr.IsDBNull(5))
                    s.scriptParameter = dr.GetString(5);
                else
                    s.scriptParameter = "";
                scripts.Add(s);
            };

            return scripts.ToArray<Command>();
        }

        public Command GetScript(int id)
        {
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = Settings.connstring
            };
            
            sqlConnection.Open();
            SqlCommand comm = new SqlCommand("select * from Commands where id = " + id.ToString(), sqlConnection);
            SqlDataReader dr = comm.ExecuteReader();
            Command ret_val = new Command();

            if (dr.Read())
            {
                ret_val.Id = dr.GetInt32(0);
                ret_val.scriptName = dr.GetString(1);
                ret_val.scriptType = dr.GetString(2);
                ret_val.scriptData = dr.GetString(3);
                if (!dr.IsDBNull(5))
                    ret_val.scriptParameter = dr.GetString(5);
                else
                    ret_val.scriptParameter = "";
            };

            return ret_val;
        }

        public bool SaveScript(Command s)
        {
            // validate

            // save to database
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = Settings.connstring
            };
            sqlConnection.Open();
            SqlCommand comm = new SqlCommand("insert into Commands (Id, Name, Type, Command) values (@id, @name, @type, @command)", sqlConnection);
            comm.Parameters.AddWithValue("@id", s.Id);
            comm.Parameters.AddWithValue("@name", s.scriptName);
            comm.Parameters.AddWithValue("@type", s.scriptType);
            comm.Parameters.AddWithValue("@command", s.scriptData);
            int i = comm.ExecuteNonQuery();

            return false;
        }

        public bool UpdateScript(Command s, int id)
        {
            // validate
            if (s.Id != id)
            {

            }

            // save to database
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = Settings.connstring
            };
            sqlConnection.Open();
            SqlCommand comm = new SqlCommand("update Commands set name = @name, type = @type, command = @command where id = @id", sqlConnection);
            comm.Parameters.AddWithValue("@name", s.scriptName);
            comm.Parameters.AddWithValue("@type", s.scriptType);
            comm.Parameters.AddWithValue("@command", s.scriptData);
            comm.Parameters.AddWithValue("@id", s.Id);

            int i = comm.ExecuteNonQuery();

            return false;
        }
    }
}