
using JWT.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JWT.Controllers
{
    [Authorize]
    public class User2Controller : ApiController
    {
        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            SqlConexion conn = new SqlConexion();
            List<User> users = new List<User>();
            DataTableReader dtr;

            try
            {
                conn.Conectar(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
                conn.PrepararProcedimiento("dbo.sp_GetAllUsers");
                dtr = conn.EjecutarTableReader();

                while (dtr.Read())
                {
                    users.Add(new User
                    {
                        Id = int.Parse(dtr["IDUser"].ToString()),
                        Name = dtr["Nombre"].ToString(),
                        LastName = dtr["Apellidos"].ToString(),
                        Nick = dtr["Nick"].ToString(),
                        Password = dtr["Password"].ToString()
                    });
                }

                return users;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Desconectar();
            }
        }

        [HttpGet]
        public IHttpActionResult GetUser(int Id)
        {
            SqlConexion conn = new SqlConexion();
            List<SqlParameter> parameters = new List<SqlParameter>();
            DataTableReader dtr;

            try
            {
                conn.Conectar(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);

                parameters.Add(new SqlParameter("@Id", Id));
                conn.PrepararProcedimiento("dbo.sp_GetUser", parameters);
                dtr = conn.EjecutarTableReader();

                if (dtr.Read())
                {
                    User searchedUser = new User
                    {
                        Id = int.Parse(dtr["IDUser"].ToString()),
                        Name = dtr["Nombre"].ToString(),
                        LastName = dtr["Apellidos"].ToString(),
                        Nick = dtr["Nick"].ToString(),
                        Password = dtr["Password"].ToString()
                    };

                    return Ok(searchedUser);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Desconectar();
            }
        }

        // Llama al método cuando se hace POST y se envía el objeto a través 
        // del encabezado de la petición HTTP (parámetros en la URL)
        [HttpPost]
        public void SaveNewUser(User newUser)
        {
            SqlConexion conn = new SqlConexion();
            List<SqlParameter> parameters = new List<SqlParameter>();

            try
            {
                conn.Conectar(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);

                parameters.Add(new SqlParameter("@Nombre", newUser.Name));
                parameters.Add(new SqlParameter("@Apellidos", newUser.LastName));
                parameters.Add(new SqlParameter("@Nick", newUser.Nick));
                parameters.Add(new SqlParameter("@Password", newUser.Password));
                conn.PrepararProcedimiento("dbo.sp_AddNewUser", parameters);
                conn.EjecutarProcedimiento();

                //return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Desconectar();
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateUserPassword([FromBody]User user)
        {
            SqlConexion conn = new SqlConexion();
            List<SqlParameter> parameters = new List<SqlParameter>();

            try
            {
                conn.Conectar(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);

                parameters.Add(new SqlParameter("@Id", user.Id));
                parameters.Add(new SqlParameter("@Password", user.Password));
                conn.PrepararProcedimiento("dbo.sp_UpdatePassword", parameters);
                conn.EjecutarProcedimiento();

                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Desconectar();
            }
        }

        [HttpDelete]
        public IHttpActionResult RemoveUser([FromBody]User user)
        {
            SqlConexion conn = new SqlConexion();
            List<SqlParameter> parameters = new List<SqlParameter>();

            try
            {
                conn.Conectar(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);

                parameters.Add(new SqlParameter("@Id", user.Id));
                conn.PrepararProcedimiento("dbo.sp_DeleteUser", parameters);
                conn.EjecutarProcedimiento();

                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Desconectar();
            }
        }
    }
}
