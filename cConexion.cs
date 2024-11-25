using System;
// agregamos la biblioteca necesaria
using System.Data.SqlClient;

namespace GestionPedidosClientes
{
    // definimos la clase como publica
    public class cConexion
    {
        // hacemos el método para la conexión al servidor que devolverá el tipo dato SqlConnection
        public SqlConnection ConexionServer()
        {
            // declaramos la variable conn de tipo de dato SqlConnection
            SqlConnection conn;
            try
            {
                // declaramos la variable de tipo string que contendrá toda la configuración de la cadena de conexión
                string cadenaConexion = "Data Source=LAPTOP-AC6UI6A3\\SQLEXPRESS;Initial Catalog=GestionPedidos;Persist Security Info=True;User ID=sa;Password=abcd";
                conn = new SqlConnection(cadenaConexion);
            }
            catch (Exception ex)
            {
                // estructura try catch por algún error
                throw new ArgumentException("Error al conectar");
            }
            return conn;
        }
    }
}
