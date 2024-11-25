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
    public partial class pedidosFiltros : Form
    {
        // instanciando y dejandolo como solo lectura clase Busquedas
        private readonly Busquedas busquedas = new Busquedas();

        public pedidosFiltros()
        {
            InitializeComponent();
        }

        public class Busquedas
        {
            // metodo constructor de la clase por defecto
            public Busquedas() { }

            // obtiene todos los registros de la tabla Pedidos
            // retorna un objeto DataTable
            public DataTable GetPedidos()
            {
                // creando un objeto que permite emular una tabla
                DataTable mydt = new DataTable();
                // instancia de la clase de conexión
                cConexion conectar = new cConexion();
                // asignando la configuración del servidor
                SqlConnection conn = conectar.ConexionServer();
                // declaración de la consulta obtener los datos de la tabla
                string sql = "SELECT idPedido,fechaPedido,fechaEntrega,comentarios,cantidad,precioUnidad,T2.producto,T1.nombre,estado FROM Pedidos T0 inner join Usuarios T1 on T0.idUsuario=T1.idUsuario inner join Productos T2 on T0.idProducto=T2.idProducto;";
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

            public DataTable FiltrarPorNombre(string nombre)
            {
                // instancia de la clase conexión
                cConexion conectar = new cConexion();
                // asignar la configuración del servidor
                SqlConnection conn = conectar.ConexionServer();

                // Sentencia SQL para filtrar por nombre
                string filtrarSql = "SELECT idPedido,fechaPedido,fechaEntrega,comentarios,cantidad,precioUnidad,T2.producto,T1.nombre,estado FROM Pedidos T0 inner join Usuarios T1 on T0.idUsuario=T1.idUsuario inner join Productos T2 on T0.idProducto=T2.idProducto WHERE T1.nombre LIKE @nombre + '%';";

                // Crear el comando SQL
                SqlCommand cmd = new SqlCommand(filtrarSql, conn);
                // parámetros que agregaremos a la sentencia sql
                cmd.Parameters.AddWithValue("@nombre", nombre);

                // Crear una tabla para almacenar los resultados
                DataTable tablaResultados = new DataTable();

                try
                {
                    // abrir conexión
                    conn.Open();

                    // Ejecutar la consulta y cargar los datos en la tabla
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(tablaResultados);
                }
                finally
                {
                    //libera el recurso
                    cmd.Dispose();
                    //cerrar la conexión
                    conn.Close();
                }

                // Devolver la tabla con los resultados
                return tablaResultados;
            }

            public DataTable FiltrarPorProducto(string producto)
            {
                // instancia de la clase conexión
                cConexion conectar = new cConexion();
                // asignar la configuración del servidor
                SqlConnection conn = conectar.ConexionServer();

                // Sentencia SQL para filtrar por producto
                string filtrarSql = "SELECT idPedido,fechaPedido,fechaEntrega,comentarios,cantidad,precioUnidad,T2.producto,T1.nombre,estado FROM Pedidos T0 inner join Usuarios T1 on T0.idUsuario=T1.idUsuario inner join Productos T2 on T0.idProducto=T2.idProducto WHERE T2.producto LIKE @producto + '%';";

                // Crear el comando SQL
                SqlCommand cmd = new SqlCommand(filtrarSql, conn);
                // parámetros que agregaremos a la sentencia sql
                cmd.Parameters.AddWithValue("@producto", producto);

                // Crear una tabla para almacenar los resultados
                DataTable tablaResultados = new DataTable();

                try
                {
                    // abrir conexión
                    conn.Open();

                    // Ejecutar la consulta y cargar los datos en la tabla
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(tablaResultados);
                }
                finally
                {
                    //libera el recurso
                    cmd.Dispose();
                    //cerrar la conexión
                    conn.Close();
                }

                // Devolver la tabla con los resultados
                return tablaResultados;
            }

            public DataTable FiltrarPorPrecio(string precio)
            {
                // instancia de la clase conexión
                cConexion conectar = new cConexion();
                // asignar la configuración del servidor
                SqlConnection conn = conectar.ConexionServer();

                // Sentencia SQL para filtrar por precio
                string filtrarSql = "SELECT idPedido,fechaPedido,fechaEntrega,comentarios,cantidad,precioUnidad,T2.producto,T1.nombre,estado FROM Pedidos T0 inner join Usuarios T1 on T0.idUsuario=T1.idUsuario inner join Productos T2 on T0.idProducto=T2.idProducto WHERE T0.precioUnidad LIKE @precio + '%';";

                // Crear el comando SQL
                SqlCommand cmd = new SqlCommand(filtrarSql, conn);
                // parámetros que agregaremos a la sentencia sql
                cmd.Parameters.AddWithValue("@precio", precio);

                // Crear una tabla para almacenar los resultados
                DataTable tablaResultados = new DataTable();

                try
                {
                    // abrir conexión
                    conn.Open();

                    // Ejecutar la consulta y cargar los datos en la tabla
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(tablaResultados);
                }
                finally
                {
                    //libera el recurso
                    cmd.Dispose();
                    //cerrar la conexión
                    conn.Close();
                }

                // Devolver la tabla con los resultados
                return tablaResultados;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Verificar que el campo de texto no esté vacío
            if (!string.IsNullOrEmpty(txtNombre.Text))
            {
                // Filtrar los resultados por nombre
                DataTable resultados = busquedas.FiltrarPorNombre(txtNombre.Text);

                // Verificar si se encontraron registros
                if (resultados.Rows.Count > 0)
                {
                    // Mostrar los resultados en el DataGridView
                    dataGridView1.DataSource = resultados;
                }
                else
                {
                    // Mostrar un mensaje si no se encontraron coincidencias
                    MessageBox.Show("No se encontraron registros con ese nombre.");
                }
            }
            else if (!string.IsNullOrEmpty(txtProducto.Text))
            {
                // Filtrar los resultados por producto
                DataTable resultados = busquedas.FiltrarPorProducto(txtProducto.Text);

                // Verificar si se encontraron registros
                if (resultados.Rows.Count > 0)
                {
                    // Mostrar los resultados en el DataGridView
                    dataGridView1.DataSource = resultados;
                }
                else
                {
                    // Mostrar un mensaje si no se encontraron coincidencias
                    MessageBox.Show("No se encontraron registros con ese producto.");
                }
            }
            else if (!string.IsNullOrEmpty(txtPrecio.Text))
            {
                // Filtrar los resultados por precio
                DataTable resultados = busquedas.FiltrarPorPrecio(txtPrecio.Text);

                // Verificar si se encontraron registros
                if (resultados.Rows.Count > 0)
                {
                    // Mostrar los resultados en el DataGridView
                    dataGridView1.DataSource = resultados;
                }
                else
                {
                    // Mostrar un mensaje si no se encontraron coincidencias
                    MessageBox.Show("No se encontraron registros con ese precio.");
                }
            }
            else
            {
                // mostrar un mensaje que complete los campos
                MessageBox.Show("Por favor, complete algun campo para buscar.");
            }
        }

        private void refrescarGrid()
        {
            // completamos el dataGridView1 con el DataTable
            dataGridView1.DataSource = busquedas.GetPedidos();
            // Configurar el formato de la columna "precioVenta" para mostrar 2 decimales
            dataGridView1.Columns["precioUnidad"].DefaultCellStyle.Format = "F2";
        }

        private void pedidosFiltros_Load(object sender, EventArgs e)
        {
            // creamos el proceso que actualiza el grid
            refrescarGrid(); // llamamos el nombre del proceso
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            //borrado
            txtNombre.Clear();
            txtProducto.Clear();
            txtPrecio.Clear();
            refrescarGrid();
            //Iniciamente asignamos como celda activa,la primera celda de la fila actual
            //Interesante para mantener la fila que estaba seleccionada visible entre las filas que nos muestra el DataGridView
            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0];
            //Procedemos a desactivar la fila seleccionada
            dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Selected = false;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            // Cierra el formulario
            this.Close();
        }
    }
}
