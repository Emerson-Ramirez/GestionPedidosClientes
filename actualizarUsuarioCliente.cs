using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// agregando biblioteca para el sql
using System.Data.SqlClient;

namespace GestionPedidosClientes
{
    public partial class actualizarUsuarioCliente : Form
    {
        // instanciando y dejandolo como solo lectura clase ActualizarCliente
        private readonly ActualizarCliente actuClientes = new ActualizarCliente();

        // variable en la cual se almacena el id proporcionado en el login
        private static int _id;

        public actualizarUsuarioCliente(int id)
        {
            InitializeComponent();
            _id = id;
        }

        // Cierra el formulario actualizarUsuarioCliente
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public class ActualizarCliente
        {
            // atributos privados de la clase
            private string usuario;
            private string contrasena;

            // metodo constructor de la clase por defecto
            public ActualizarCliente() { }

            // metodo constructor que acepta parametros
            public ActualizarCliente(string usuario, string contrasena)
            {
                this.usuario = usuario;
                this.contrasena = contrasena;
            }

            // propiedades publicas para cada atributo
            public int Id { get { return _id; } set { _id = value; } }
            public string Usuario { get { return usuario; } set { usuario = value; } }
            public string Contrasena { get { return contrasena; } set { contrasena = value; } }

            // obtiene todos los registros de la tabla Usuarios
            // retorna valores de tipo objeto
            public object Informacion()
            {
                object info;
                // instancia de la clase conexión
                cConexion conectar = new cConexion();
                // asignar la configuración del servidor
                SqlConnection conn = conectar.ConexionServer();
                // sentencia de busqueda
                string sqlInfo = "select usuario +','+ contrasena from Usuarios where idUsuario = @idUsuario";
                SqlCommand cmd = new SqlCommand(sqlInfo, conn);
                // parámetros que agregaremos a la sentencia sql
                cmd.Parameters.AddWithValue("@idUsuario", this.Id);
                try
                {
                    // abrir conexión
                    conn.Open();
                    // capturamos el escalar y ejecutamos la sentencia
                    info = cmd.ExecuteScalar();
                }
                finally
                {
                    //libera el recurso
                    cmd.Dispose();
                    //cerrar la conexión
                    conn.Close();
                }
                return info;
            }

            // actualiza un registro de la tabla Usuarios
            // retorna el numero del registro afectado
            public int EditarUsuario()
            {
                int affected = 0; // para determinar si se actulizo la base
                // instancia de la clase conexión
                cConexion conectar = new cConexion();
                // asignar la configuración del servidor
                SqlConnection conn = conectar.ConexionServer();
                // estructura de actualizar
                string updateSql = "update Usuarios set usuario=@usuario,contrasena=@contrasena where idUsuario=@idUsuario;";
                SqlCommand cmd = new SqlCommand(updateSql, conn);
                // parámetros o campos que se actualizan
                cmd.Parameters.AddWithValue("@usuario", this.usuario);
                cmd.Parameters.AddWithValue("@contrasena", this.contrasena);
                cmd.Parameters.AddWithValue("@idUsuario", this.Id);
                try
                {
                    // abrir conexión
                    conn.Open();
                    // ejecutación de la actualización y captura de los registros modificados 
                    affected = cmd.ExecuteNonQuery();
                }
                finally
                {
                    //libera el recurso
                    cmd.Dispose();
                    //cerrar la conexión
                    conn.Close();
                }
                return affected;
            }
        }

        public void MostrarInformacion()
        {
            // se separa del arreglo de dos valores separados por una coma
            string[] valores = actuClientes.Informacion().ToString().Split(',');
            // variable que se utiliza para almacenar la busqueda del usuario
            string usuario = valores[0];
            // variable que se utiliza para almacenar la busqueda de la contraseña
            string contrasena = valores[1];
            // se le atribuye a los textbox el resultado de las varibles anteriores para ser mostrados en la interfaz
            txtMostrarUsuario.Text = usuario;
            txtMostrarContrasena.Text = contrasena;
        }

        private void actualizarUsuarioCliente_Load(object sender, EventArgs e)
        {
            // creamos el proceso que muestra los datos en la interfaz
            MostrarInformacion(); // llamamos el nombre del proceso
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            // asignamos los valores a las propiedades
            actuClientes.Usuario = txtUsuario.Text.Trim();
            actuClientes.Contrasena = txtContrasena.Text.Trim();

            int affected = actuClientes.EditarUsuario();
            if (affected > 0)//confirmamos que actualice registros
            {
                MessageBox.Show("Credenciales Actualizadas Exitosamente");//mensaje de actualizar
                // cerramos el formulario
                this.Close();
            }
        }
    }
}
