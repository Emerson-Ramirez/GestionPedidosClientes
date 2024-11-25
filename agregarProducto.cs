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
    public partial class agregarProducto : Form
    {
        // instanciando y dejandolo como solo lectura clase Productos
        private readonly Productos productos = new Productos();

        public agregarProducto()
        {
            InitializeComponent();
        }

        public class Productos
        {
            // atributos privados de la clase
            private long idProducto;
            private string producto;
            private string descripcion;
            private int cantidadStock;
            private decimal precioVenta;
            private DateTime fecha;

            // metodo constructor de la clase por defecto
            public Productos() { }

            // metodo constructor que acepta parametros
            public Productos(long idProducto, string producto, string descripcion, int cantidadStock, decimal precioVenta, DateTime fecha)
            {
                this.idProducto = idProducto;
                this.producto = producto;
                this.descripcion = descripcion;
                this.cantidadStock = cantidadStock;
                this.precioVenta = precioVenta;
                this.fecha = fecha;
            }

            // propiedades publicas para cada atributo
            public long IdProducto { get { return idProducto; } set { idProducto = value; } }
            public string Producto { get {return producto; } set { producto = value; } }
            public string Descripcion { get {return descripcion; } set { descripcion = value; } }
            public int CantidadStock { get { return cantidadStock; } set { cantidadStock = value; } }
            public decimal PrecioVenta { get { return precioVenta; } set { precioVenta = value; } }
            public DateTime Fecha { get { return fecha; } set { fecha = value; } }

            // obtiene todos los registros de la tabla Productos
            // retorna un objeto DataTable
            public DataTable GetProductos()
            {
                // creando un objeto que permite emular una tabla
                DataTable mydt = new DataTable();
                // instancia de la clase de conexión
                cConexion conectar = new cConexion();
                // asignando la configuración del servidor
                SqlConnection conn = conectar.ConexionServer();
                // declaración de la consulta obtener los datos de la tabla
                string sql = "select idProducto,producto,descripcion,cantidadStock,precioVenta,fecha from Productos;";
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

            // agrega un registro de la tabla Productos
            // retorna el correlativo del ultimo registro agregado
            public long AgregarProducto()
            {
                long id = 0; // variable para capturar el valor escalar
                // instancia de la clase conexión
                cConexion conectar = new cConexion();
                // asignar la configuración del servidor
                SqlConnection conn = conectar.ConexionServer();
                // declaraciones de la consulta insert para agregar los datos a la tabla
                // IDENT_CURRENT('Productos') devuleve el ultimo valor agregado
                string insertSql = "insert into Productos(producto,descripcion,cantidadStock,precioVenta,fecha) values (@producto,@descripcion,@cantidadStock,@precioVenta,@fecha);select ident_current('Productos') as id";
                SqlCommand cmd = new SqlCommand(insertSql, conn);
                // parámetros que agregaremos a la sentencia sql
                cmd.Parameters.AddWithValue("@producto", this.producto);
                cmd.Parameters.AddWithValue("@descripcion", this.descripcion);
                cmd.Parameters.AddWithValue("@cantidadStock", this.cantidadStock);
                cmd.Parameters.AddWithValue("@precioVenta", this.precioVenta);
                cmd.Parameters.AddWithValue("@fecha", this.fecha);
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

            // actualiza un registro de la tabla Productos
            // retorna el numero del registro afectado
            public int EditarProducto()
            {
                int affected = 0; // para determinar si se actulizo la base
                // instancia de la clase conexión
                cConexion conectar = new cConexion();
                // asignar la configuración del servidor
                SqlConnection conn = conectar.ConexionServer();
                // estructura de actualizar
                string updateSql = "update Productos set producto=@producto,descripcion=@descripcion,cantidadStock=@cantidadStock,precioVenta=@precioVenta,fecha=@fecha where idProducto=@idProducto;";
                SqlCommand cmd = new SqlCommand(updateSql, conn);
                // parámetros o campos que se actualizan
                cmd.Parameters.AddWithValue("@producto", this.producto);
                cmd.Parameters.AddWithValue("@descripcion", this.descripcion);
                cmd.Parameters.AddWithValue("@cantidadStock", this.cantidadStock);
                cmd.Parameters.AddWithValue("@precioVenta", this.precioVenta);
                cmd.Parameters.AddWithValue("@fecha", this.fecha);
                cmd.Parameters.AddWithValue("@idProducto", this.idProducto);
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

            // elimina un registro de la tabla Productos
            // retorna el numero del registro afectado
            public int EliminarProducto()
            {
                // variable para confirmar si eliminamos registro en la base de datos
                int affected = 0;
                // instancia de la clase conexión
                cConexion conectar = new cConexion();
                // asignar la configuración del servidor
                SqlConnection conn = conectar.ConexionServer();
                // estructura de borrado
                string deleteSql = "delete from Productos where idProducto=@idProducto;";
                SqlCommand cmd = new SqlCommand(deleteSql, conn);
                // agregando el parámetro que se necesita eliminar en la base de datos
                cmd.Parameters.AddWithValue("@idProducto", this.idProducto);
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

        private void agregarProducto_Load(object sender, EventArgs e)
        {
            // creamos el proceso que actualiza el grid
            refrescarGrid(); // llamamos el nombre del proceso
        }

        private void refrescarGrid()
        {
            // completamos el dataGridView1 con el DataTable
            dataGridView1.DataSource = productos.GetProductos();
            // Configurar el formato de la columna "precioVenta" para mostrar 2 decimales
            dataGridView1.Columns["precioVenta"].DefaultCellStyle.Format = "F2";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // validamos que no esten vacios los campos de entradas
            if (!string.IsNullOrEmpty(txtProducto.Text) && !string.IsNullOrEmpty(txtDescripcion.Text) && !string.IsNullOrEmpty(numCantidad.Text) && !string.IsNullOrEmpty(txtPrecio.Text) && !string.IsNullOrEmpty(dateFechaIngreso.Text))
            {
                // asignamos los valores a las propiedades
                productos.Producto = txtProducto.Text.Trim();
                productos.Descripcion = txtDescripcion.Text.Trim();
                productos.CantidadStock = (int)numCantidad.Value;
                productos.PrecioVenta = Math.Round(decimal.Parse(txtPrecio.Text),2);
                productos.Fecha = DateTime.Parse(dateFechaIngreso.Text);

                long id = productos.AgregarProducto();
                if (id > 0)//confirmamos si agregamos un registro
                {
                    MessageBox.Show("Producto Agregado Exitosamente");//presentamos el mensaje de agregado
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
                // asignamos el valor de idProducto, la fila contiene celdas y esta en la posición 0
                txtId.Text = row.Cells[0].Value.ToString();
                // llamando cada una de las celdas del grid
                txtProducto.Text = row.Cells["producto"].Value.ToString();
                txtDescripcion.Text = row.Cells["descripcion"].Value.ToString();//notar que es el nombre de cada columna
                numCantidad.Text = row.Cells["cantidadStock"].Value.ToString();

                // Mostrar precioVenta con 2 decimales
                if (decimal.TryParse(row.Cells["precioVenta"].Value.ToString(), out decimal precio))
                {
                    txtPrecio.Text = precio.ToString("F2"); // Formatear con 2 decimales
                }
                else
                {
                    txtPrecio.Text = "0.00"; // Valor predeterminado si no es un número válido
                }

                //txtPrecio.Text = row.Cells["precioVenta"].Value.ToString();
                dateFechaIngreso.Text = row.Cells["fecha"].Value.ToString();
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!txtId.Text.Equals(""))//validando que no este vacio
            {
                //obteniendo el identificador unico del registro
                string id = dataGridView1.CurrentRow.Cells["idProducto"].Value.ToString();
                if (!string.IsNullOrEmpty(id))
                {
                    // asignamos los valores a las propiedades
                    productos.IdProducto = Convert.ToInt64(id);
                    productos.Producto = txtProducto.Text.Trim();
                    productos.Descripcion = txtDescripcion.Text.Trim();
                    productos.CantidadStock = (int)numCantidad.Value;
                    productos.PrecioVenta = Math.Round(decimal.Parse(txtPrecio.Text),2);
                    productos.Fecha = DateTime.Parse(dateFechaIngreso.Text);

                    int affected = productos.EditarProducto();
                    if (affected > 0)//confirmamos que actualice registros
                    {
                        MessageBox.Show("Producto Actualizado Exitosamente");//mensaje de actualizar
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
                string id = dataGridView1.CurrentRow.Cells["idProducto"].Value.ToString();
                if (!string.IsNullOrEmpty(id))
                {
                    productos.IdProducto = Convert.ToInt64(id);
                    //variable para confirmar si eliminamos registro de la base de datos
                    int affected = productos.EditarProducto();
                    if (affected >= 1)//confirmamos que eliminamos registros
                    {
                        MessageBox.Show("Producto Eliminado Exitosamente");//mensaje de eliminacion
                        refrescarGrid();//refrescar grid
                    }
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            //borrado
            txtProducto.Clear();
            txtDescripcion.Clear();
            numCantidad.ResetText();
            txtPrecio.Clear();
            dateFechaIngreso.ResetText();
            txtProducto.Focus();
            txtId.Clear();
            //Iniciamente asignamos como celda activa,la primera celda de la fila actual
            //Interesante para mantener la fila que estaba seleccionada visible entre las filas que nos muestra el DataGridView
            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0];
            //Procedemos a desactivar la fila seleccionada
            dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Selected = false;
        }

        // Cierra el formulario agregarProducto
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}