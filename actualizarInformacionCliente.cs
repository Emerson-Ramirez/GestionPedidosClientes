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
    public partial class actualizarInformacionCliente : Form
    {
        // instanciando y dejandolo como solo lectura clase ActualizarCliente
        private readonly ActualizarInformacion actuClientes = new ActualizarInformacion();

        // variable en la cual se almacena el id proporcionado en el login
        private static int _id;

        public actualizarInformacionCliente(int id)
        {
            InitializeComponent();
            _id = id;
        }

        public class ActualizarInformacion
        {
            // atributos privados de la clase
            private string nombre;
            private decimal telefono;
            private string direccion;
            private string departamento;
            private string ciudad;

            // metodo constructor de la clase por defecto
            public ActualizarInformacion() { }

            // metodo constructor que acepta parametros
            public ActualizarInformacion(string nombre, decimal telefono, string direccion, string departamento, string ciudad)
            {
                this.nombre = nombre;
                this.telefono = telefono;
                this.direccion = direccion;
                this.departamento = departamento;
                this.ciudad = ciudad;
            }

            // propiedades publicas para cada atributo
            public int Id { get { return _id; } set { _id = value; } }
            public string Nombre { get { return nombre; } set { nombre = value; } }
            public decimal Telefono { get { return telefono; } set { telefono = value; } }
            public string Direccion { get { return direccion; } set { direccion = value; } }
            public string Departamento { get { return departamento; } set { departamento = value; } }
            public string Ciudad { get { return ciudad; } set { ciudad = value; } }

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
                string sqlInfo = "select isnull(nombre,'') +','+ isnull(cast(telefono as varchar),'') +','+ isnull(direccion,'') +','+ isnull(departamento,'') +','+ isnull(ciudad,'') from Usuarios where idUsuario = @idUsuario;";
                SqlCommand cmd = new SqlCommand(sqlInfo, conn);
                // parámetro que agregaremos a la sentencia sql
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
                string updateSql = "update Usuarios set nombre=@nombre,telefono=@telefono,direccion=@direccion,departamento=@departamento,ciudad=@ciudad where idUsuario=@idUsuario;";
                SqlCommand cmd = new SqlCommand(updateSql, conn);
                // parámetros o campos que se actualizan
                cmd.Parameters.AddWithValue("@nombre", this.nombre);
                cmd.Parameters.AddWithValue("@telefono", this.telefono);
                cmd.Parameters.AddWithValue("@direccion", this.direccion);
                cmd.Parameters.AddWithValue("@departamento", this.departamento);
                cmd.Parameters.AddWithValue("@ciudad", this.ciudad);
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
            string nombre = valores[0];
            // variable que se utiliza para almacenar la busqueda de la contraseña
            string telefono = valores[1];
            // variable que se utiliza para almacenar la busqueda de la direccion
            string direccion = valores[2];
            // variable que se utiliza para almacenar la busqueda del departamento
            string departamento = valores[3];
            // variable que se utiliza para almacenar la busqueda de la ciudad
            string ciudad = valores[4];
            // se le atribuye a los textbox el resultado de las varibles anteriores para ser mostrados en la interfaz
            txtMostrarNombre.Text = nombre;
            txtMostrarTelefono.Text = telefono;
            txtMostrarDireccion.Text = direccion;
            txtMostrarDepartamento.Text = departamento;
            txtMostrarCiudad.Text = ciudad;
        }

        private void actualizarInformacionCliente_Load(object sender, EventArgs e)
        {
            // creamos el proceso que muestra los datos en la interfaz
            MostrarInformacion(); // llamamos el nombre del proceso
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            // validamos que no esten vacios los campos de entradas
            if (!string.IsNullOrEmpty(txtNombre.Text) && !string.IsNullOrEmpty(txtTelefono.Text) && !string.IsNullOrEmpty(txtDireccion.Text) && !string.IsNullOrEmpty(txtDepartamento.Text) && !string.IsNullOrEmpty(txtCiudad.Text))
            {
                // asignamos los valores a las propiedades
                actuClientes.Nombre = txtNombre.Text.Trim();
                actuClientes.Telefono = Convert.ToDecimal(txtTelefono.Text.Trim());
                actuClientes.Direccion = txtDireccion.Text.Trim();
                actuClientes.Departamento = txtDepartamento.Text.Trim();
                actuClientes.Ciudad = txtCiudad.Text.Trim();

                int affected = actuClientes.EditarUsuario();
                if (affected > 0)//confirmamos que actualice registros
                {
                    MessageBox.Show("Información Actualizada Exitosamente");//mensaje de actualizar
                    // cerramos el formulario
                    this.Close();
                }
            } 
        }

        // Cierra el formulario actualizarInformacionCliente
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
