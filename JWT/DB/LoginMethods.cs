using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace JWT.DB
{
    public class LoginMethods
    {
        public static string GetUserPassword(string username)
        {
            SqlConexion conexion = new SqlConexion();
            List<SqlParameter> parameters = new List<SqlParameter>();
            DataTableReader dtr;

            try
            {
                conexion.Conectar(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);

                parameters.Add(new SqlParameter("@Nick", username));
                conexion.PrepararProcedimiento("dbo.sp_GetUserByNick", parameters);

                dtr = conexion.EjecutarTableReader();

                dtr.Read();
                return dtr["Password"].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conexion.Desconectar();
            }
        }
    }
}