using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace VictusWebService
{
    /// <summary>
    /// Summary description for VictusWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class VictusWS : System.Web.Services.WebService
    {
        static string Cadena_Conexion = "Data Source=.;Initial Catalog=VictusWeb;Integrated Security=True";
        static SqlConnection Conexion = new SqlConnection(Cadena_Conexion);

        // Metodos basicos de BD
        public DataTable Ejecutar_Consulta(StringBuilder Query, SqlCommand Comando = null)
        {
            // Es mejor usar un DataTable cuando trabajemos con consultas a una sola tabla.
            DataTable Tabla = new DataTable();
            try
            {
                Tabla.TableName = "Consulta";
                Conexion.Open();
                if (Comando == null)
                {
                    Comando = new SqlCommand();
                }
                Comando.Connection = Conexion;
                Comando.CommandType = CommandType.Text;
                Comando.CommandText = Query.ToString();

                SqlDataReader Lector = Comando.ExecuteReader();
                // DataReader para tabajar DataTable!

                Tabla.Load(Lector);
                Conexion.Close();

                return Tabla;
            }
            catch (Exception ex)
            {
                Conexion.Close();
                throw new Exception(ex.ToString());
                //throw new Exception("Error en Capa de Datos: " + ex);
                // En el mundo real mostrar el mensaje de error no es lo optimo, pues puede ser informacion sensible.
            }
        }

        public int EjecutarSentencia(StringBuilder Query, SqlCommand Comando = null)
        {
            // Es mejor usar parametros especificos para evitar vulnerabilidades.
            // Aca tambien definimos a Comando como null para que sea Opcinal.
            int resultado = 0;

            try
            {
                Conexion.Open();
                Comando.Connection = Conexion;
                Comando.CommandType = CommandType.Text;
                Comando.CommandText = Query.ToString();

                resultado = Comando.ExecuteNonQuery();
                // Captura el numero de fials afectadas por este comando.

                Conexion.Close();

                return resultado;
            }
            catch (Exception ex)
            {
                Conexion.Close();
                throw new Exception(ex.ToString());
                //throw new Exception("Error en Capa de Datos: " + ex);
            }

        }
        #region Informacion de Usuario
        // Registro de un usuario.
        [WebMethod]
        public int InsertarInformacionPersona(int cedula,string correo,string nombre,string primerApellido, string segundoApellido, bool genero, string clave)
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            int i;
            try
            {
                sqlQuery.Append("INSERT INTO [Informacion.Persona] VALUES (@cedula, @correo, @nombre, @primerApellido, @segundoApellido,@genero,@clave)");

                comando.Parameters.Add("@cedula",SqlDbType.Int).Value = cedula;
                comando.Parameters.Add("@correo",SqlDbType.NVarChar).Value = correo;
                comando.Parameters.Add("@nombre", SqlDbType.NVarChar).Value = nombre;
                comando.Parameters.Add("@primerApellido", SqlDbType.NVarChar).Value = primerApellido;
                comando.Parameters.Add("@segundoApellido", SqlDbType.NVarChar).Value = segundoApellido;
                comando.Parameters.Add("@genero",SqlDbType.Bit).Value = genero;
                comando.Parameters.Add("@clave",SqlDbType.NVarChar).Value = clave;

                i = EjecutarSentencia(sqlQuery, comando);
                return i;
            }
            catch (Exception e)
            {
                throw new Exception("Error:" + e);
            }
        }
        // Informacion personal de Usuario.
        [WebMethod]
        public int InsertarInformacionPersonal(int cedula, int edad, double altura, double peso, double imc,int cantidadVasos, DateTime fechaDatos) {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            int i;
            try
            {
                sqlQuery.Append("INSERT INTO [Informacion.Personal] VALUES (@cedula, @edad, @altura, @peso, @imc, @cantidadVasos, @fechaDatos)");
                comando.Parameters.Add("@cedula",SqlDbType.Int).Value = cedula;
                comando.Parameters.Add("@edad",SqlDbType.Int).Value = edad;
                comando.Parameters.Add("@altura",SqlDbType.Int).Value = altura;
                comando.Parameters.Add("@peso",SqlDbType.Decimal).Value = peso;
                comando.Parameters.Add("@imc",SqlDbType.Decimal).Value = imc;
                comando.Parameters.Add("@cantidadVasos",SqlDbType.Int).Value = cantidadVasos;
                comando.Parameters.Add("@fechaDatos",SqlDbType.DateTime).Value = fechaDatos;

                i = EjecutarSentencia(sqlQuery, comando);
                return i;
            }
            catch (Exception e)
            {
                throw new Exception("Error:" + e);
            }
        }
        // Obtener la informacion mas reciente del Usuario
        [WebMethod]
        public DataTable ObtenerInformacionPersonal(int cedula) {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();
            try
            {
                sqlQuery.Append("SELECT MAX(FechaDatos) AS 'UltimoRegistro' FROM [Informacion.Personal] WHERE Cedula = @cedula");
                comando.Parameters.Add("@cedula", SqlDbType.Int).Value = cedula;

                // Se obtiene la fecha de la informacion personal mas reciente.
                tabla = Ejecutar_Consulta(sqlQuery, comando);
                if (tabla.Rows.Count > 0)
                {
                    // Con esa fecha, se reaaliza la consulta por la informacion.
                    tabla = ObtenerUltimaInformacionPersonal(cedula, Convert.ToDateTime(tabla.Rows[0][0]));
                }
                return tabla;
            }
            catch (Exception e)
            {
                throw new Exception("Error:" + e);
            }
        }

        // Retorna la informacion personal del usuario.
        public DataTable ObtenerUltimaInformacionPersonal(int cedula, DateTime fechaDatos) {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();
            try
            {
                sqlQuery.Append("SELECT * FROM [Informacion.Personal] WHERE Cedula = @cedula AND FechaDatos = @fechaDatos");
                comando.Parameters.Add("@cedula", SqlDbType.Int).Value = cedula;
                comando.Parameters.Add("@fechaDatos", SqlDbType.Date).Value = fechaDatos;

                tabla = Ejecutar_Consulta(sqlQuery, comando);
                return tabla;
            }
            catch (Exception e)
            {
                throw new Exception("Error:" + e);
            }
        }
        //[WebMethod]
        //public int InsertarMedicion() {
        //}
        #endregion

    }
}
