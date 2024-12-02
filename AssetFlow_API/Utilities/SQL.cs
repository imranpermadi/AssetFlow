using FAMS_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FAMS_API.Utilities
{
    public class SQL
    {
       
        public static string ExecuteQuery(String query,
                                          Dictionary<string, object> parameters,
                                          Boolean useExistingConnection = false,
                                          SqlConnection conn = null,
                                          SqlTransaction trans = null)
        {
            DataEntities db = new DataEntities();
            string connString = db.Database.Connection.ConnectionString;
            try
            {
                if (string.IsNullOrEmpty(connString))
                {
                    throw new Exception("Connection is invalid");
                }


                if (!useExistingConnection || conn == null)
                {
                    conn = new SqlConnection(connString);
                    conn.Open();
                }

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    if (useExistingConnection && trans != null) command.Transaction = trans;

                    foreach (KeyValuePair<string, object> parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                    command.CommandTimeout = 6000;
                    command.ExecuteNonQuery();
                }

                if (!useExistingConnection && trans != null)
                {
                    trans.Commit();
                }
            }
            catch (Exception exc)
            {
                if (!useExistingConnection && trans != null)
                {
                    trans.Rollback();
                }
                return exc.Message;
            }
            finally
            {
                if (!useExistingConnection)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return "OK";
        }

        public static string GetDataTable(String query,
                                          Dictionary<string, object> parameters,
                                          out DataTable data,
                                          Boolean useExistingConnection = false,
                                          SqlConnection conn = null,
                                          SqlTransaction trans = null,
                                          bool increaseTimeOut = false)
        {
            DataEntities db = new DataEntities();
            data = new DataTable();
            string connString = db.Database.Connection.ConnectionString;

            try
            {
                if (string.IsNullOrEmpty(connString))
                {
                    throw new Exception("Connection is invalid");
                }

                if (!useExistingConnection || conn == null)
                {
                    conn = new SqlConnection(connString);
                    conn.Open();
                }

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    if (useExistingConnection && trans != null) command.Transaction = trans;
                    if (increaseTimeOut || true)
                        command.CommandTimeout = 300;
                    foreach (KeyValuePair<string, object> parameter in parameters)
                    {
                        //command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                        if (parameter.Value.GetType() == typeof(string))
                        {
                            command.Parameters.Add(new SqlParameter(parameter.Key, SqlDbType.VarChar, parameter.Value.ToString().Length, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Default, parameter.Value));
                        }
                        else
                            command.Parameters.AddWithValue(parameter.Key, parameter.Value);

                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(data);
                    }
                }
            }
            catch (Exception exc)
            {
                return exc.Message;
            }
            finally
            {
                if (!useExistingConnection)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return "OK";
        }
    }
}
