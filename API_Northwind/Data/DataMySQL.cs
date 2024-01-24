using API_Northwind.Model;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using System.Transactions;

namespace API_Northwind.Data
{
    public class DataMySQL
    {
        public readonly string _ServiceDatabase = "";

        public DataMySQL(IConfiguration configuration) {

            _ServiceDatabase = configuration.GetConnectionString("ConnectionDataBase");
        
        }


        public DataTable ExecuteGet(string SProcedureName, List<ENT_Parameter> param = null)
        {
            using (MySqlConnection conexion = new MySqlConnection(_ServiceDatabase))
            {
                try
                {
                    conexion.Open();
                    MySqlCommand cmd = new MySqlCommand(SProcedureName, conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (param != null)
                    {
                        foreach (var value in param)
                        {
                            cmd.Parameters.AddWithValue(value.Nombre, value.Valor);
                        }
                    }
                    DataTable tableResult = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(tableResult);

                    return tableResult;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }


        public dynamic ExecuteStoredProcedure(string NameSProcedure, List<ENT_Parameter> param = null) 
        {
            MySqlConnection conexion = new MySqlConnection(_ServiceDatabase);
            MySqlTransaction transaction = null;

            try
            {
                conexion.Open();
                transaction = conexion.BeginTransaction();

                MySqlCommand cmd = new MySqlCommand(NameSProcedure, conexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Transaction = transaction;

                if (param != null)
                {
                    foreach (var value in param)
                    {
                        cmd.Parameters.AddWithValue(value.Nombre, value.Valor);
                    }
                }

                cmd.ExecuteNonQuery();

                transaction.Commit();

                return new
                {
                    Process = true,
                    Message = "Proceso exitoso"
                };


            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return new
                {
                    Proceso = false,
                    Mensaje = ex.Message
                };
            }
            finally
            {
                conexion.Close();
            }
        }


        public dynamic ExecuteParameter(string NameSProcedure, string Id)
        {
            MySqlConnection conexion = new MySqlConnection(_ServiceDatabase);
            MySqlTransaction transaction = null;

            try
            {
                conexion.Open();
                transaction = conexion.BeginTransaction();

                MySqlCommand cmd = new MySqlCommand(NameSProcedure, conexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Transaction = transaction;

                if (Id != null)
                {
                    cmd.Parameters.AddWithValue("id", Id);

                }

                int i = cmd.ExecuteNonQuery();

                transaction.Commit();

                return new
                {
                    Process = true,
                    Message = "Operacion exitosa, registro eliminado"
                };

            }
            catch (Exception ex)
            {
                return new
                {
                    Proceso = false,
                    Mensaje = ex.Message
                };
            }
            finally
            {
                conexion.Close();
            }

        }


        public DataTable ExecuteGetId(string SProcedureName, int id)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conexion = new MySqlConnection(_ServiceDatabase))
            {
                try
                {
                    conexion.Open();
                    
                    MySqlCommand cmd = new MySqlCommand(SProcedureName, conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("@Id", id));

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                    da.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

    }
}

