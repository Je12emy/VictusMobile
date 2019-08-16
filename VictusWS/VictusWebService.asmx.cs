using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace VictusWS
{
    /// <summary>
    /// Summary description for VictusWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class VictusWebService : System.Web.Services.WebService
    {

        #region Metodos de BD
        static string Cadena_Conexion = "workstation id=VictusWeb.mssql.somee.com;packet size=4096;user id=Jel2emy_SQLLogin_1;pwd=zmyxyipbde;data source=VictusWeb.mssql.somee.com;persist security info=False;initial catalog=VictusWeb";
        static SqlConnection Conexion = new SqlConnection(Cadena_Conexion);


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
        #endregion

        #region Entidad Persona

        // Entidad para almacenar la informacion basica de registro de cada usuario.
        [WebMethod]
        public DataTable BuscarUsuario(string Correo)
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            sqlQuery.Append("SELECT Correo, Contraseña FROM Persona WHERE Correo = @Correo ");

            comando.Parameters.Add("@Correo", SqlDbType.NVarChar).Value = Correo;
            tabla = Ejecutar_Consulta(sqlQuery, comando);
            return tabla;
        }
        [WebMethod]
        public DataTable BuscarUsuarioTodo(string Correo)
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();
          
            sqlQuery.Append("SELECT * FROM Persona WHERE Correo = @Correo");

            comando.Parameters.Add("@Correo", SqlDbType.NVarChar).Value = Correo;

            tabla = Ejecutar_Consulta(sqlQuery,comando);
            return tabla;
        }
        [WebMethod]
        public int CrearUsuario(string correo, int cedula, string nombre, string apellido1, string apellido2, bool genero, string contraseña)
        {

            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            int i;

            sqlQuery.Append("INSERT INTO Persona Values(@Correo,@Cedula,@Nombre,@Apellido1,@Apellido2,@Genero,@Contraseña)");

            comando.Parameters.Add("@Correo", SqlDbType.NVarChar).Value = correo;
            comando.Parameters.Add("@Cedula", SqlDbType.Int).Value = cedula;
            comando.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = nombre;
            comando.Parameters.Add("@Apellido1", SqlDbType.NVarChar).Value = apellido1;
            comando.Parameters.Add("@Apellido2", SqlDbType.NVarChar).Value = apellido2;
            comando.Parameters.Add("@Genero", SqlDbType.Bit).Value = genero;
            comando.Parameters.Add("@Contraseña", SqlDbType.NVarChar).Value = contraseña;

            i = EjecutarSentencia(sqlQuery,comando);
            return i;
        }
        #endregion

        [WebMethod]
        #region Entidad Cliente
        public DataTable BuscarCliente(string correo, DateTime fecha)
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            comando.CommandText = "SELECT * FROM Cliente where Correo = @Correo and FechaDatos = @Fecha";

            comando.Parameters.Add("@Correo", SqlDbType.NVarChar).Value = correo;
            comando.Parameters.Add("@Fecha", SqlDbType.NVarChar).Value = fecha;

            tabla = Ejecutar_Consulta(sqlQuery,comando);
            return tabla;
        }

        [WebMethod]
        public DataTable BuscarUltimoRegistro(string correo)
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            sqlQuery.Append("Select Max(FechaDatos) as UltimoRegistro from Cliente Where Correo = @Correo");

            comando.Parameters.Add("@Correo", SqlDbType.NVarChar).Value = correo;
           
            tabla = Ejecutar_Consulta(sqlQuery,comando);
            return tabla;
        }

        [WebMethod]
        public int AgregarDatosCliente(string Correo, double Peso, string Altura, string Edad,double IMC, string Agua, DateTime Fecha)
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            int i;

            sqlQuery.Append("INSERT INTO Cliente Values(@Correo, @Peso, @Altura, @Edad, @IMC, @Agua, @Fecha)");

            comando.Parameters.Add("@Correo", SqlDbType.NVarChar).Value = Correo;
            comando.Parameters.Add("@Peso", SqlDbType.Float).Value = Peso;
            comando.Parameters.Add("@Altura", SqlDbType.Int).Value = Altura;
            comando.Parameters.Add("@Edad", SqlDbType.Int).Value = Edad;
            comando.Parameters.Add("@IMC", SqlDbType.Float).Value = IMC;
            comando.Parameters.Add("@Agua", SqlDbType.Int).Value = Agua;
            comando.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = Fecha;

            i = EjecutarSentencia(sqlQuery,comando);
            return i;
        }
        #endregion

        #region Entidad Harris Ben
        [WebMethod]
        public DataTable BuscarUltimoRegistroHarris(string correo)
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();
           
            sqlQuery.Append("Select Max(FechaHarris) as UltimoRegistro from HarrisBen Where Correo = @Correo");

            comando.Parameters.Add("@Correo", SqlDbType.NVarChar).Value = correo;

            tabla = Ejecutar_Consulta(sqlQuery,comando);
            return tabla;
        }
        [WebMethod]
        public DataTable BuscarRegistroHarris(string correo, DateTime fecha)
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            sqlQuery.Append("SELECT * FROM HarrisBen where Correo = @Correo and FechaHarris = @Fecha");

            comando.Parameters.Add("@Correo", SqlDbType.NVarChar).Value = correo;
            comando.Parameters.Add("@Fecha", SqlDbType.NVarChar).Value = fecha;          

            tabla = Ejecutar_Consulta(sqlQuery,comando);
            return tabla;
        }
        [WebMethod]
        public int AgregarRegistroHarris(double FactorActividad, double TMB, double NivelCalorico, DateTime fecha, string correo)
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            int i;

            sqlQuery.Append("INSERT INTO HarrisBen Values(@FactorActividad, @TMB, @NivelCalorico, @FechaHarris, @Correo)");

            comando.Parameters.Add("@FactorActividad", SqlDbType.Float).Value = FactorActividad;
            comando.Parameters.Add("@TMB", SqlDbType.Float).Value = TMB;
            comando.Parameters.Add("@NivelCalorico", SqlDbType.Float).Value = NivelCalorico;
            comando.Parameters.Add("@FechaHarris", SqlDbType.DateTime).Value = fecha;
            comando.Parameters.Add("@Correo", SqlDbType.NVarChar).Value = correo;

            i = EjecutarSentencia(sqlQuery,comando);
            return i;
        }
        #endregion

        #region Entidad Dieta
        [WebMethod]
        public int AgregarDieta(string CorreoCliente, DateTime FechaDieta, int CodigoHarris, string Objetivo)
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            int i;

            sqlQuery.Append("INSERT INTO Dieta Values(@CorreoCliente, @FechaDieta, @CodigoHarris, @Objetivo)");

            comando.Parameters.Add("@CorreoCliente", SqlDbType.NVarChar).Value = CorreoCliente;
            comando.Parameters.Add("@FechaDieta", SqlDbType.DateTime).Value = FechaDieta;
            comando.Parameters.Add("@CodigoHarris", SqlDbType.Int).Value = CodigoHarris;
            comando.Parameters.Add("@Objetivo", SqlDbType.NVarChar).Value = Objetivo;

            i = EjecutarSentencia(sqlQuery,comando);
            return i;
        }
        [WebMethod]
        public DataTable ObtenerUltimaDieta(string CorreoCliente)
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();
            
            sqlQuery.Append("Select Max(FechaDieta) as UltimaDieta from Dieta Where CorreoCliente = @Correo");
            comando.Parameters.Add("@Correo", SqlDbType.NVarChar).Value = CorreoCliente;

            tabla = Ejecutar_Consulta(sqlQuery,comando);
            return tabla;
        }
        [WebMethod]
        public DataTable ObtenerDieta(string CorreoCliente, DateTime FechaDieta)
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            DataTable table = new DataTable();
       
            sqlQuery.Append("SELECT * FROM Dieta WHERE CorreoCliente = @Correo AND FechaDieta = @Fecha");
            comando.Parameters.Add("@Correo", SqlDbType.NVarChar).Value = CorreoCliente;
            comando.Parameters.Add("@Fecha", SqlDbType.NVarChar).Value = CorreoCliente;

            table = Ejecutar_Consulta(sqlQuery,comando);
            return table;
        }

        #endregion

        #region Entidad-DietaRelacion
        [WebMethod]
        public int AgregarRelacion(int CodigoDieta, int CodigoAlimento)
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            int i;

            sqlQuery.Append("INSERT INTO RelacionAlimentos Values(@CodigoDieta, @CodigoAlimento)");
            comando.Parameters.Add("@CodigoDieta", SqlDbType.Int).Value = CodigoDieta;
            comando.Parameters.Add("@CodigoAlimento", SqlDbType.Int).Value = CodigoAlimento;

            i = EjecutarSentencia(sqlQuery,comando);
            return i;
        }
        #endregion

        #region Entidad Catalogo Alimentos
        [WebMethod]
        public DataTable ObtenerCatalogoAlimentos()
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand(); ;
            DataTable table = new DataTable(); ;

            sqlQuery.Append("SELECT * FROM CatalogoAlimentos");

            table = Ejecutar_Consulta(sqlQuery,comando);
            return table;
        }
        [WebMethod]
        public DataTable ObtenerDietaCompleta(string CorreoCliente, int CodigoDieta)
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            DataTable table;

            sqlQuery.Append("EXEC GetDieta @CorreoCliente, @CodigoDieta");

            comando.Parameters.Add("@CorreoCliente", SqlDbType.NVarChar).Value = CorreoCliente;
            comando.Parameters.Add("@CodigoDieta", SqlDbType.Int).Value = CodigoDieta;

            table = Ejecutar_Consulta(sqlQuery,comando);
            return table;
        }
        #endregion

        #region Medidas
        [WebMethod]
        public int AgregarMedidas(DateTime FechaMedida, string CorreoCliente, double BicepIzquierdo, double BicepDerecho, double Abdomen, double CuadricepIzquierdo, double CuadricepDerecho, double PantorrillaIzquierda, double PantorrillaDerecha)
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            int i;

            sqlQuery.Append("INSERT INTO Medida Values(@FechaMedida,@CorreoCliente, @BicepIzquierdo, @BicepDerecho, @Abdomen, @CuadricepIzquierdo, @CuadricepDerecho, @PantorrillaIzquierda, @PantorrillaDerecha)");
            comando.Parameters.Add("@FechaMedida", SqlDbType.DateTime).Value = FechaMedida;
            comando.Parameters.Add("@CorreoCliente", SqlDbType.NVarChar).Value = CorreoCliente;
            comando.Parameters.Add("@BicepIzquierdo", SqlDbType.Float).Value = BicepIzquierdo;
            comando.Parameters.Add("@BicepDerecho", SqlDbType.Float).Value = BicepDerecho;
            comando.Parameters.Add("@Abdomen", SqlDbType.Float).Value = Abdomen;
            comando.Parameters.Add("@CuadricepIzquierdo", SqlDbType.Float).Value = CuadricepIzquierdo;
            comando.Parameters.Add("@CuadricepDerecho", SqlDbType.Float).Value = CuadricepDerecho;
            comando.Parameters.Add("@PantorrillaIzquierda", SqlDbType.Float).Value = PantorrillaIzquierda;
            comando.Parameters.Add("@PantorrillaDerecha", SqlDbType.Float).Value = PantorrillaDerecha;

            i = EjecutarSentencia(sqlQuery,comando);
            return i;
        }
        [WebMethod]
        public DataTable ObtenerUltimaMedida(string CorreoCliente)
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            sqlQuery.Append("Select Max(FechaMedida) as UltimaMedida from Medida Where CorreoCliente = @Correo");
            comando.Parameters.Add("@Correo", SqlDbType.NVarChar).Value = CorreoCliente;

            tabla = Ejecutar_Consulta(sqlQuery,comando);
            return tabla;
        }
        [WebMethod]
        public DataTable ObtenerMedida(string CorreoCliente, DateTime FechaDieta)
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            sqlQuery.Append("SELECT * FROM Medida WHERE CorreoCliente = @Correo AND FechaMedida = @Fecha");
            comando.Parameters.Add("@Correo", SqlDbType.NVarChar).Value = CorreoCliente ;
            comando.Parameters.Add("@Fecha", SqlDbType.NVarChar).Value = FechaDieta;

            tabla = Ejecutar_Consulta(sqlQuery,comando);
            return tabla;
        }
        [WebMethod]
        public DataTable ObtenerMedicionCompleta(int CodigoMedida)
        {
            StringBuilder sqlQuery = new StringBuilder();
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            comando.Parameters.Add("@CodigoMedida", SqlDbType.Int).Value = CodigoMedida ;
            sqlQuery.Append("SELECT medicion.BicepIzquierdo, medicion.BicepDerecho, medicion.Abdomen, medicion.CuadricepIzquierdo," +
                                        "medicion.CuadricepDerecho, medicion.PantorrillaIzquierda, " +
                                        "medicion.PantorrillaDerecha FROM Medida AS medicion WHERE CodigoMedida =  @CodigoMedida");

            tabla = Ejecutar_Consulta(sqlQuery,comando);
            return tabla;
        }

        #endregion
    }
}
