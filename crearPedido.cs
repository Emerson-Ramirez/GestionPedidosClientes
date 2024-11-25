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
    public partial class crearPedido : Form
    {
        // instanciando y dejandolo como solo lectura clase Pedidos
        private readonly Pedidos pedidos = new Pedidos();

        // variable en la cual se almacena el id proporcionado en el login
        private static int _id;

        public crearPedido(int id)
        {
            InitializeComponent();
            _id = id;
        }

        public class Pedidos
        {
            // atributos privados de la clase
            private long idProducto;
            private int cantidad;
            private DateTime fechaPedido;
            private DateTime fechaEntrega;
            private string comentarios;
            private decimal precioUnidad;

            // metodo constructor de la clase por defecto
            public Pedidos() { }

            // metodo constructor que acepta parametros
            public Pedidos(int idProducto, int cantidad, DateTime fechaPedido, DateTime fechaEntrega, string comentarios, decimal precioUnidad)
            {
                this.idProducto = idProducto;
                this.cantidad = cantidad;
                this.fechaPedido = fechaPedido;
                this.fechaEntrega = fechaEntrega;
                this.comentarios = comentarios;
                this.precioUnidad = precioUnidad;
            }

            // propiedades publicas para cada atributo
            public int IdUsuario { get { return _id; } set { _id = value; } }
            public long IdProducto { get { return idProducto; } set { idProducto = value; } }
            public int Cantidad { get { return cantidad; } set { cantidad = value; } }
            public DateTime FechaPedido { get { return fechaPedido; } set { fechaPedido = value; } }
            public DateTime FechaEntrega { get { return fechaEntrega; } set { fechaEntrega = value; } }
            public string Comentarios { get { return comentarios; } set { comentarios = value; } }
            public decimal PrecioUnidad { get { return precioUnidad; } set { precioUnidad= value; } }

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

            // agrega un registro de la tabla Pedidos
            // retorna el correlativo del ultimo registro agregado
            public long AgregarPedido()
            {
                long id = 0; // variable para capturar el valor escalar
                // instancia de la clase conexión
                cConexion conectar = new cConexion();
                // asignar la configuración del servidor
                SqlConnection conn = conectar.ConexionServer();
                // declaraciones de la consulta insert para agregar los datos a la tabla
                // IDENT_CURRENT('Pedidos') devuleve el ultimo valor agregado
                string insertSql = "insert into Pedidos(fechaPedido,fechaEntrega,comentarios,cantidad,precioUnidad,idProducto,idUsuario,estado) values (@fechaPedido,@fechaEntrega,@comentarios,@cantidad,@precioUnidad,@idProducto,@idUsuario,'C');select ident_current('Pedidos') as id";
                SqlCommand cmd = new SqlCommand(insertSql, conn);
                // parámetros que agregaremos a la sentencia sql
                cmd.Parameters.AddWithValue("@fechaPedido", this.fechaPedido);
                cmd.Parameters.AddWithValue("@fechaEntrega", this.fechaEntrega);
                cmd.Parameters.AddWithValue("@comentarios", this.comentarios);
                cmd.Parameters.AddWithValue("@cantidad", this.cantidad);
                cmd.Parameters.AddWithValue("@precioUnidad", this.precioUnidad);
                cmd.Parameters.AddWithValue("@idProducto", this.idProducto);
                cmd.Parameters.AddWithValue("@idUsuario", this.IdUsuario);
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
            public int ActualizarStock()
            {
                int affected = 0; // para determinar si se actulizo la base
                // instancia de la clase conexión
                cConexion conectar = new cConexion();
                // asignar la configuración del servidor
                SqlConnection conn = conectar.ConexionServer();
                // sentecia sql que busca la cantidad de stock del producto
                string busquedaStockSql = "select cantidadStock from Productos where idProducto = @idProducto;";
                SqlCommand cmd = new SqlCommand(busquedaStockSql, conn);
                // parámetros o campos que se actualizan
                cmd.Parameters.AddWithValue("@idProducto", this.idProducto);
                try
                {
                    // abrir conexión
                    conn.Open();
                    // capturamos el escalar y ejecutamos la sentencia 
                    int cantidadStock = Convert.ToInt32(cmd.ExecuteScalar());
                    // se crea una condicion
                    if (cantidadStock >= Cantidad)
                    {
                        // variable que tendra ya la actualización del stock del producto
                        int cantidadStockActualizada = cantidadStock - Cantidad;
                        // sentencia sql para actualizar
                        string actualizarStockSql = "update Productos set cantidadStock = @cantidadStock where idProducto = @idProducto;";
                        SqlCommand cmd2 = new SqlCommand(actualizarStockSql, conn);
                        // parámetros o campos que se actualizan
                        cmd2.Parameters.AddWithValue("@cantidadStock", cantidadStockActualizada);
                        cmd2.Parameters.AddWithValue("@idProducto", this.idProducto);
                        // ejecutación de la actualización y captura de los registros modificados 
                        affected = cmd2.ExecuteNonQuery();
                    }
                    else
                    {
                        // mensaje de prevencion
                        MessageBox.Show("Cantidad deseada no disponible");
                    }
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

        private void refrescarGrid()
        {
            // completamos el dataGridView1 con el DataTable
            dataGridView1.DataSource = pedidos.GetProductos();
            // Configurar el formato de la columna "precioVenta" para mostrar 2 decimales
            dataGridView1.Columns["precioVenta"].DefaultCellStyle.Format = "F2";
        }

        private void crearPedido_Load(object sender, EventArgs e)
        {
            // creamos el proceso que actualiza el grid
            refrescarGrid(); // llamamos el nombre del proceso
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

                // Mostrar precioVenta con 2 decimales
                if (decimal.TryParse(row.Cells["precioVenta"].Value.ToString(), out decimal precio))
                {
                    txtPrecio.Text = precio.ToString("F2"); // Formatear con 2 decimales
                }
                else
                {
                    txtPrecio.Text = "0.00"; // Valor predeterminado si no es un número válido
                }
            }
        }

        private void btnAgregarCarrito_Click(object sender, EventArgs e)
        {
            if (!txtId.Text.Equals("") && !txtComentario.Text.Equals(""))//validando que no este vacio
            {
                //obteniendo el identificador unico del registro
                string idP = dataGridView1.CurrentRow.Cells["idProducto"].Value.ToString();
                if (!string.IsNullOrEmpty(idP))
                {
                    // asignamos los valores a las propiedades
                    pedidos.IdProducto = Convert.ToInt64(idP);
                    pedidos.Cantidad = (int)numCantidad.Value;

                    int affected = pedidos.ActualizarStock();
                    if (affected > 0)//confirmamos que actualice registros
                    {
                        // asignamos los valores a las propiedades
                        pedidos.FechaPedido = DateTime.Parse(fechaPedido.Text);
                        pedidos.FechaEntrega = DateTime.Parse(fechaEntrega.Text);
                        pedidos.Comentarios = txtComentario.Text.Trim();
                        pedidos.PrecioUnidad = Math.Round(decimal.Parse(txtPrecio.Text), 2);
                        long id = pedidos.AgregarPedido();
                        if (id > 0) //confirmamos que agregue el registros
                        {
                            MessageBox.Show("Agregado Exitosamente");//mensaje de agregar
                            refrescarGrid();//refrescar el contenido del grid
                        }
                        // borramos
                        numCantidad.ResetText();
                    }
                }
            }
            else
            {
                // mensaje preventivo
                MessageBox.Show("Completar los campos");
            }
        }

        private void btnVerCarrito_Click(object sender, EventArgs e)
        {
            // abre el formulario carrito
            carrito verCarrito = new carrito(_id);
            // muestra el formulario
            verCarrito.Show();
        }

        // Cierra el formulario crearPedido
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}