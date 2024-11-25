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
    public partial class agregarUsuario : Form
    {
        // instanciando y dejandolo como solo lectura clase Usuarios
        private readonly Usuarios usuarios = new Usuarios();

        public agregarUsuario()
        {
            InitializeComponent();
        }

        public class Usuarios
        {
            // atributos privados de la clase
            private long idUsuario;
            private string nombre;
            private string usuario;
            private string contrasena;
            private string tipo;

            // metodo constructor de la clase por defecto
            public Usuarios() { }

            // metodo constructor que acepta parametros
            public Usuarios(long idUsuario, string nombre, string usuario, string contrasena, string tipo)
            {
                this.idUsuario = idUsuario;
                this.nombre = nombre;
                this.usuario = usuario;
                this.contrasena = contrasena;
                this.tipo = tipo;
            }

            // propiedades publicas para cada atributo
            public long IdUsuario { get { return idUsuario; } set { idUsuario = value; } }
            public string Nombre { get { return nombre; } set { nombre = value; } }
            public string Usuario { get { return usuario; } set { usuario = value; } }
            public string Contrasena { get { return contrasena; } set { contrasena = value; } }
            public string Tipo { get { return tipo; } set { tipo = value; } }

            // obtiene todos los registros de la tabla Usuarios
            // retorna un objeto DataTable
            public DataTable GetUsuarios()
            {
                // creando un objeto que permite emular una tabla
                DataTable mydt = new DataTable();
                // instancia de la clase de conexión
                cConexion conectar = new cConexion();
                // asignando la configuración del servidor
                SqlConnection conn = conectar.ConexionServer();
                // declaración de la consulta obtener los datos de la tabla
                string sql = "select idUsuario,nombre,usuario,contrasena,tipoUsuario from Usuarios where tipoUsuario in ('A','O');";
                SqlCommand cmd = new SqlCommand(sql, conn);
                // para lectura de la tabla
                SqlDataReader mydr = null;
                try
                {
                    //abrir la conexión
                    conn.Open();
                    // ejecución de la consulta y lectura de la tabla
                    mydr = cmd.ExecuteReader();
                    // agregando la lectura hecha a mi tabla
                    mydt.Load(mydr);
                }
                finally
                {
                    //libera el recurso
                    cmd.Dispose();
                    //cerrar la conexión
                    conn.Close();
                }
                return mydt;
            }

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
                string insertSql = "insert into Usuarios(nombre,usuario,contrasena,tipoUsuario) values (@nombre,@usuario,@contrasena,@tipoUsuario);select ident_current('Usuarios') as id";
                SqlCommand cmd = new SqlCommand(insertSql, conn);
                // parámetros que agregaremos a la sentencia sql
                cmd.Parameters.AddWithValue("@nombre", this.nombre);
                cmd.Parameters.AddWithValue("@usuario", this.usuario);
                cmd.Parameters.AddWithValue("@contrasena", this.contrasena);
                cmd.Parameters.AddWithValue("@tipoUsuario", this.tipo);
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
                string updateSql = "update Usuarios set nombre=@nombre,usuario=@usuario,contrasena=@contrasena,tipoUsuario=@tipoUsuario where idUsuario=@idUsuario;";
                SqlCommand cmd = new SqlCommand(updateSql, conn);
                // parámetros o campos que se actualizan
                cmd.Parameters.AddWithValue("@nombre", this.nombre);
                cmd.Parameters.AddWithValue("@usuario", this.usuario);
                cmd.Parameters.AddWithValue("@contrasena", this.contrasena);
                cmd.Parameters.AddWithValue("@tipoUsuario", this.tipo);
                cmd.Parameters.AddWithValue("@idUsuario", this.idUsuario);
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

            // elimina un registro de la tabla Usuarios
            // retorna el numero del registro afectado
            public int EliminarUsuario()
            {
                // variable para confirmar si eliminamos registro en la base de datos
                int affected = 0;
                // instancia de la clase conexión
                cConexion conectar = new cConexion();
                // asignar la configuración del servidor
                SqlConnection conn = conectar.ConexionServer();
                // estructura de borrado
                string deleteSql = "delete from Usuarios where idUsuario=@idUsuario;";
                SqlCommand cmd = new SqlCommand(deleteSql, conn);
                // agregando el parámetro que se necesita eliminar en la base de datos
                cmd.Parameters.AddWithValue("@idUsuario", this.idUsuario);
                try
                {
                    // abrir conexión
                    conn.Open();
                    // ejecutación de la actualización y captura de los registros eliminados 
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

        private void agregarUsuario_Load(object sender, EventArgs e)
        {
            // creamos el proceso que actualiza el grid
            refrescarGrid(); // llamamos el nombre del proceso
        }

        private void refrescarGrid()
        {
            // completamos el dataGridView1 con el DataTable
            dataGridView1.DataSource = usuarios.GetUsuarios();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // validamos que no esten vacios los campos de entradas
            if (!string.IsNullOrEmpty(txtNombre.Text) && !string.IsNullOrEmpty(txtUsuario.Text) && !string.IsNullOrEmpty(txtContrasena.Text) && !string.IsNullOrEmpty(txtTipo.Text))
            {
                // asignamos los valores a las propiedades
                usuarios.Nombre = txtNombre.Text.Trim();
                usuarios.Usuario = txtUsuario.Text.Trim();
                usuarios.Contrasena = txtContrasena.Text.Trim();
                usuarios.Tipo = txtTipo.Text.Trim();

                long id = usuarios.AgregarUsuario();
                if (id > 0)//confirmamos si agregamos un registro
                {
                    MessageBox.Show("Usuario Agregado Exitosamente");//presentamos el mensaje de agregado
                    refrescarGrid();//llamamos el proceso refrescar
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //Asegurarse que el usuario seleccione al menos 1 fila
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // creamos variable para capturar la fila actual
                var row = dataGridView1.CurrentRow;
                // asignamos el valor de idUsuario, la fila contiene celdas y esta en la posición 0
                txtId.Text = row.Cells[0].Value.ToString();
                // llamando cada una de las celdas del grid
                txtNombre.Text = row.Cells["nombre"].Value.ToString();
                txtUsuario.Text = row.Cells["usuario"].Value.ToString();//notar que es el nombre de cada columna
                txtContrasena.Text = row.Cells["contrasena"].Value.ToString();
                txtTipo.Text = row.Cells["tipoUsuario"].Value.ToString();
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!txtId.Text.Equals(""))//validando que no este vacio
            {
                //obteniendo el identificador unico del registro
                string id = dataGridView1.CurrentRow.Cells["idUsuario"].Value.ToString();
                if (!string.IsNullOrEmpty(id))
                {
                    // asignamos los nuevos valores a editar a las propiedades
                    usuarios.IdUsuario = Convert.ToInt64(id);
                    usuarios.Nombre = txtNombre.Text.Trim();
                    usuarios.Usuario = txtUsuario.Text.Trim();
                    usuarios.Contrasena = txtContrasena.Text.Trim();
                    usuarios.Tipo = txtTipo.Text.Trim();

                    int affected = usuarios.EditarUsuario();
                    if (affected > 0)//confirmamos que elimine registros
                    {
                        MessageBox.Show("Usuario Actualizado Exitosamente");//mensaje de actualizar
                        refrescarGrid();//refrescar el contenido del grid
                    }
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            //no permitir la actualizacion si el valor de txtId es vacio
            if (!txtId.Text.Equals(""))
            {
                //obteniendo el identificador unico del registro
                string id = dataGridView1.CurrentRow.Cells["idUsuario"].Value.ToString();
                if (!string.IsNullOrEmpty(id))
                {
                    usuarios.IdUsuario = Convert.ToInt64(id);
                    //variable para confirmar si eliminamos registro de la base de datos
                    int affected = usuarios.EliminarUsuario();
                    if (affected >= 1)//confirmamos que eliminamos registros
                    {
                        MessageBox.Show("Usuario Eliminado Exitosamente");//mensaje de eliminacion
                        refrescarGrid();//refrescar grid
                    }
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            //borrado
            txtNombre.Clear();
            txtUsuario.Clear();
            txtContrasena.Clear();
            txtTipo.Clear();
            txtNombre.Focus();
            txtId.Clear();
            //Iniciamente asignamos como celda activa,la primera celda de la fila actual
            //Interesante para mantener la fila que estaba seleccionada visible entre las filas que nos muestra el DataGridView
            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0];
            //Procedemos a desactivar la fila seleccionada
            dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Selected = false;
        }

        // Cierra el formulario agregarUsuario
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}