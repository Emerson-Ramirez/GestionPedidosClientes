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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace GestionPedidosClientes
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        // Esta región de codigo sirve para quitar o vaciar el texto del textbox para poder empezar a escribir
        #region quitar o vaciar el texto del textbox
        private void txtUser_Enter(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "USUARIO")
            {
                txtUsuario.Text = "";
                txtUsuario.ForeColor = Color.LightGray;
            }
        }

        private void txtPass_Enter(object sender, EventArgs e)
        {
            if (txtContrasena.Text == "CONTRASEÑA")
            {
                txtContrasena.Text = "";
                txtContrasena.ForeColor = Color.LightGray;
                txtContrasena.UseSystemPasswordChar = true;
            }
        }
        #endregion

        // Esta región de codigo sirve para volver a poner el texto del textbox respectivo
        #region volver a poner el texto del textbox respectivo
        private void txtUser_Leave(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "")
            {
                txtUsuario.Text = "USUARIO";
                txtUsuario.ForeColor = Color.DimGray;
            }
        }

        private void txtPass_Leave(object sender, EventArgs e)
        {
            if (txtContrasena.Text == "")
            {
                txtContrasena.Text = "CONTRASEÑA";
                txtContrasena.ForeColor = Color.DimGray;
                txtContrasena.UseSystemPasswordChar = false;
            }
        }
        #endregion

        #region Botones
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            // Cierra la aplicación o formulario
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            // Minimiza el aplicación o formulario
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnAcceder_Click(object sender, EventArgs e)
        {
            // variables que contienen los datos ingresados por la persona
            string usuario = txtUsuario.Text;
            string contrasena = txtContrasena.Text;
            // instancia de la clase de conexión
            cConexion conectar = new cConexion();
            // asignando la configuración del servidor
            SqlConnection conn = conectar.ConexionServer();
            // abrimos la conexión
            conn.Open();
            // sentencia sql de busqueda del tipo de usuario
            string sql = "SELECT cast(idUsuario as varchar) +','+ tipoUsuario FROM Usuarios WHERE usuario = @usuario AND contrasena = @contrasena";
            // Crear el comando SQL
            SqlCommand cmd = new SqlCommand(sql, conn);
            // parametros
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue("@contrasena", contrasena);
            // resultado de la busqueda
            var resulto = cmd.ExecuteScalar();

            if (resulto != null)
            {
                // se separa del arreglo de dos valores separados por una coma
                string[] valores = resulto.ToString().Split(',');
                // variable que se utiliza para almacenar la busqueda del id del usuario
                int idUsuario = int.Parse(valores[0]);
                // variable que se utiliza para almacenar la busqueda del tipo de usuario
                string tipoUsuario = valores[1];
                // comparativas para saber si es administrador u operador
                if (tipoUsuario == "A")
                {
                    MessageBox.Show("Bienvenido, Administrador");
                    // Abrir formulario de administrador
                    var adminForm = new MenuAdministrador();
                    adminForm.Show();
                    adminForm.FormClosed += Logout;
                    // Oculta el formulario de login
                    this.Hide();
                }
                else if (tipoUsuario == "O")
                {
                    MessageBox.Show("Bienvenido, Operador");
                    // Abrir formulario de operador
                    var operadorForm = new MenuOperador();
                    operadorForm.Show();
                    operadorForm.FormClosed += Logout;
                    // Oculta el formulario de login
                    this.Hide();
                }
                else if (tipoUsuario == "C")
                {
                    MessageBox.Show("Bienvenido, Cliente");
                    // Abrir formulario de cliente con el parametro del id del usuario que esta ingresando
                    var clienteForm = new MenuCliente(idUsuario);
                    clienteForm.Show();
                    clienteForm.FormClosed += Logout;
                    // Oculta el formulario de login
                    this.Hide();

                }
                else if(tipoUsuario != "A" || tipoUsuario != "O")
                {
                    MessageBox.Show("Tipo de usuario registrado inválido");
                }
            }
            else
            {
                // Mensaje de advertencia
                MessageBox.Show("Credenciales incorrectas");
            }
        }
        #endregion

        // cerrar sesion
        private void Logout(object sender, FormClosedEventArgs e)
        {
            // restaurar los textos de los campos
            txtUsuario.Text = "USUARIO";
            txtContrasena.Text = "CONTRASEÑA";
            txtContrasena.UseSystemPasswordChar = false;
            // muestra el formulario login de nuevo
            this.Show();
        }

        // crear nuevo cliente
        private void btnCliente_Click(object sender, EventArgs e)
        {
            // abrir formulario para crear credenciales al nuevo cliente
            Cliente cliente = new Cliente();
            cliente.Show();
            cliente.FormClosed += Logout;
            // Oculta el formulario de login
            this.Hide();
        }

        // Mostrar integrantes del proyecto
        private void btnIntegrantes_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Integrantes:"+"\n\r"+"- Jose Manuel Panameño Campos"
                +"\n\r"+"- Diego Alejandro Rivera Muñoz"+"\n\r"+"- Luis Alberto Sánchez Platero"
                +"\n\r"+"- Bryan Antonio Guzmán Flores"+"\n\r"+"- Emerson René López Ramírez");
        }
    }
}