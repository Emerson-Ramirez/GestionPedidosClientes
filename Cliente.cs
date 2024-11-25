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
    public partial class Cliente : Form
    {
        // instanciando y dejandolo como solo lectura clase LoginCliente
        private readonly LoginCliente cliente = new LoginCliente();

        public Cliente()
        {
            InitializeComponent();
        }

        public class LoginCliente
        {
            // atributos privados de la clase
            private long idUsuario;
            private string usuario;
            private string contrasena;

            // metodo constructor de la clase por defecto
            public LoginCliente() { }

            // metodo constructor que acepta parametros
            public LoginCliente(long idUsuario, string usuario, string contrasena)
            {
                this.idUsuario = idUsuario;
                this.usuario = usuario;
                this.contrasena = contrasena;
            }

            // propiedades publicas para cada atributo
            public long IdUsuario { get { return idUsuario; } set { idUsuario = value; } }
            public string Usuario { get {return usuario; } set { usuario = value; } }
            public string Contrasena { get { return contrasena; } set { contrasena = value; } }

            // agrega un registro de la tabla Usuarios
            // retorna el correlativo del ultimo registro agregado
            public long AgregarUsuario()
            {
                long id = 0; // variable para capturar el valor escalar
                // instancia de la clase conexión
                cConexion conectar = new cConexion();
                // asignar la configuración del servidor
                SqlConnection conn = conectar.ConexionServer();
                // declaraciones de la consulta insert para agregar los datos a la tabla
                // IDENT_CURRENT('Usuarios') devuleve el ultimo valor agregado
                string insertSql = "insert into Usuarios(nombre,usuario,contrasena,tipoUsuario) values ('',@usuario,@contrasena,'C');select ident_current('Usuarios') as id";
                SqlCommand cmd = new SqlCommand(insertSql, conn);
                // parámetros que agregaremos a la sentencia sql
                cmd.Parameters.AddWithValue("@usuario", this.usuario);
                cmd.Parameters.AddWithValue("@contrasena", this.contrasena);
                try
                {
                    // abrir conexión
                    conn.Open();
                    // capturamos el escalar y ejecutamos la sentencia 
                    id = Convert.ToInt64(cmd.ExecuteScalar());
                }
                finally
                {
                    //libera el recurso
                    cmd.Dispose();
                    //cerrar la conexión
                    conn.Close();
                }
                return id;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // validamos que no esten vacios los campos de entradas
            if (!string.IsNullOrEmpty(txtUsuario.Text) && !string.IsNullOrEmpty(txtContrasena.Text))
            {
                // asignamos los valores a las propiedades
                cliente.Usuario = txtUsuario.Text.Trim();
                cliente.Contrasena = txtContrasena.Text.Trim();

                long id = cliente.AgregarUsuario();
                if (id > 0)//confirmamos si agregamos un registro
                {
                    MessageBox.Show("Usuario Agregado Exitosamente");//presentamos el mensaje de agregado
                    // cerramos el formulario
                    this.Close();
                }
            }
        }

        // Cierra el formulario Cliente
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}