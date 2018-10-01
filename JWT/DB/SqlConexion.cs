using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Descripción breve de SqlConexion
/// </summary>
public class SqlConexion
{
    private SqlConnection _conn = null;
    string _ConnectionString = "";//no se usa
    bool _Conectado = false;

    string _NombreProcedimiento = "";
    List<SqlParameter> _Parametros = new List<SqlParameter>();
    bool _Preparado = false;

    public bool Conectar(string ConnectionString)
    {
        bool _Respuesta = false;
        _conn = new SqlConnection(ConnectionString);

        try
        {
            _conn.Open();
            _Conectado = true;
            _Respuesta = true;
        }
        catch (SqlException SqlEx)
        {
            string MensajeError = "ERROR: " + SqlEx.Message + ". " + "LINEA: " + SqlEx.LineNumber + ".";
            throw new Exception(MensajeError, SqlEx);
        }
        catch
        {
            _Respuesta = false;
        }
        return _Respuesta;
    }

    public void Desconectar()
    {
        try
        {
            _conn.Close();
        }
        catch
        {
        }
    }

    public void PrepararProcedimiento(string NombreProcedimiento, List<SqlParameter> Parametros)
    {
        if (_Conectado)
        {
            // Se limpian los atributos en caso de que haya algo en memoria
            _NombreProcedimiento = "";
            _Parametros.Clear();

            _NombreProcedimiento = NombreProcedimiento;
            _Parametros = Parametros;

            // Bandera que indica que el procedimiento almacenado está listo para ejecutarse
            _Preparado = true;
        }
        else
        {
            throw new Exception("No hay conexion con la BD");
        }
    }

    public void PrepararProcedimiento(string NombreProcedimiento)
    {
        if (_Conectado)
        {
            // Se limpian los atributos en caso de que haya algo en memoria
            _NombreProcedimiento = "";
            _Parametros.Clear();

            _NombreProcedimiento = NombreProcedimiento;

            // Bandera que indica que el procedimiento almacenado está listo para ejecutarse
            _Preparado = true;
        }
        else
        {
            throw new Exception("No hay conexion con la BD");
        }
    }

    public DataTableReader EjecutarTableReader()
    {
        if (_Preparado)
        {
            DataTable dataTable = new DataTable();
            SqlCommand command = new SqlCommand(_NombreProcedimiento, _conn);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 120; // SEGUNDOS DE ESPERA PARA EJECUTAR UNA CONSULTA EN SQL

            if (_Parametros.Any())
            {
                command.Parameters.AddRange(_Parametros.ToArray());
            }

            SqlDataAdapter adapterDataTable = new SqlDataAdapter(command);
            adapterDataTable.Fill(dataTable);
            _Preparado = false;

            return dataTable.CreateDataReader();
        }
        else
        {
            _Preparado = false;
            throw new Exception("Procedimiento no preparado");
        }
    }

    public int EjecutarProcedimiento()
    {
        if (_Preparado)
        {
            SqlCommand command = new SqlCommand(_NombreProcedimiento, _conn);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 120;

            if (_Parametros.Any())
            {
                command.Parameters.AddRange(_Parametros.ToArray());
            }

            _Preparado = false;
            return command.ExecuteNonQuery();
        }
        else
        {
            _Preparado = false;
            throw new Exception("Procedimiento no preparado");
        }
    }
}