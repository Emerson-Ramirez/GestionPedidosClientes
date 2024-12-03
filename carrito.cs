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
    public partial class carrito : Form
    {
        // variable en la cual se almacena el id proporcionado en el login
        private int _id;

        public carrito(int id)
        {
            InitializeComponent();
            _id = id;
        }

        // obtiene los registros de las tablas Pedidos y Productos
        // retorna un objeto DataTable
        public DataTable GetPedido()
        {
            // creando un objeto que permite emular una tabla
            DataTable mydt = new DataTable();
            // instancia de la clase de conexión
            cConexion conectar = new cConexion();
            // asignando la configuración del servidor
            SqlConnection conn = conectar.ConexionServer();
            // declaración de la consulta obtener los datos de la tabla
            string sql = "select T1.producto as 'Nombre del producto',T1.descripcion as 'Descripción',T0.precioUnidad as 'Precio por unidad',T0.cantidad as 'Cantidad',T0.cantidad*T0.precioUnidad as 'Total' from Pedidos T0 inner join Productos T1 on T0.idProducto = T1.idProducto where T0.estado = 'C' and T0.idUsuario = @idUsuario;";
            SqlCommand cmd = new SqlCommand(sql, conn);
            // parámetros que agregaremos a la sentencia sql
            cmd.Parameters.AddWithValue("@idUsuario", _id);
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

        public decimal calculo()
        {
            // variable para determinar si se realizo el calculo
            decimal total = 0;
            // instancia de la clase de conexión
            cConexion conectar = new cConexion();
            // asignando la configuración del servidor
            SqlConnection conn = conectar.ConexionServer();
            // sentencia sql que realiza el calculo
            string calculo = "select sum(cantidad*precioUnidad) as 'Total' from Pedidos where estado = 'C' and idUsuario = @idUsuario;";
            SqlCommand cmd = new SqlCommand(calculo, conn);
            // parámetros que agregaremos a la sentencia sql
            cmd.Parameters.AddWithValue("@idUsuario", _id);
            try
            {
                //abrir la conexión
                conn.Open();
                // resultado de la busqueda
                var result = cmd.ExecuteScalar();
                // se crea una condicional y se convierte el resultado anterior a tipo decimal saliendo este por una nueva variable del mismo tipo
                if (result != null && decimal.TryParse(result.ToString(), out decimal totalC))
                {
                    // se le asigna el valor calculado y convertido
                    total = totalC;
                }
            }
            finally
            {
                //libera el recurso
                cmd.Dispose();
                //cerrar la conexión
                conn.Close();
            }
            return total;
        }

        private int ActualizarEstado()
        {
            int affected = 0; // para determinar si se actulizo la base
            // instancia de la clase conexión
            cConexion conectar = new cConexion();
            // asignar la configuración del servidor
            SqlConnection conn = conectar.ConexionServer();
            // estructura de actualizar
            string updateSql = "update Pedidos set estado='F' where estado='C' and idUsuario=@idUsuario;";
            SqlCommand cmd = new SqlCommand(updateSql, conn);
            // parámetros o campos que se actualizan
            cmd.Parameters.AddWithValue("@idUsuario", _id);
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
        
        private string BuscarNombre()
        {
            string nombre = null; // para determinar si se realiza la operacion
            // instancia de la clase conexión
            cConexion conectar = new cConexion();
            // asignar la configuración del servidor
            SqlConnection conn = conectar.ConexionServer();
            // estructura de actualizar
            string updateSql = "select nombre from Usuarios where idUsuario=@idUsuario;";
            SqlCommand cmd = new SqlCommand(updateSql, conn);
            // parámetros o campos que se actualizan
            cmd.Parameters.AddWithValue("@idUsuario", _id);
            try
            {
                // abrir conexión
                conn.Open();
                // ejecutación de la actualización y captura del registro 
                nombre = cmd.ExecuteScalar().ToString();
            }
            finally
            {
                //libera el recurso
                cmd.Dispose();
                //cerrar la conexión
                conn.Close();
            }
            return nombre;
        }

        private void refrescarGrid()
        {
            // completamos el dataGridView1 con el DataTable
            dataGridView1.DataSource = GetPedido();
            // Configurar el formato de la columna "precioVenta" para mostrar 2 decimales
            dataGridView1.Columns["Precio por unidad"].DefaultCellStyle.Format = "F2";
            // Configurar el formato de la columna "precioVenta" para mostrar 2 decimales
            dataGridView1.Columns["Total"].DefaultCellStyle.Format = "F2";
            // Configurar el formato del resultado del calculo realizado para mostrar 2 decimales
            txtTotal.Text = calculo().ToString("F2");
        }

        private void carrito_Load(object sender, EventArgs e)
        {
            // creamos el proceso que actualiza el grid
            refrescarGrid(); // llamamos el nombre del proceso
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            // Minimiza el aplicación o formulario
            WindowState = FormWindowState.Minimized;
        }

        private void btnFinalizarPedido_Click(object sender, EventArgs e)
        {
            // varible la cual se calcula el precio total del pedido y se configura para mostrar 2 decimales
            string totalPagar = calculo().ToString("F2");
            int id = ActualizarEstado();
            if (id > 0) // confirmamos la actualizacion del estado del pedido
            {
                // variable por la cual se obtiene el nombre del cliente
                string nombre = BuscarNombre();
                if (nombre != null) // confirmamos
                {
                    // se muestra un mensaje simulando una factura el cual muestra el nombre del cliente y el total a pagar por el pedido
                    MessageBox.Show("Cliente: "+nombre+"."+"\r\n"+"Total a pagar: $"+totalPagar);
                    // refresca el grid
                    refrescarGrid();
                }
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            // Cierra el formulario
            this.Close();
        }
    }
}
