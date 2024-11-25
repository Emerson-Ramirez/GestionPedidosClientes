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
    public partial class verProductos : Form
    {
        // instanciando y dejandolo como solo lectura clase Usuarios
        private readonly Productos productos = new Productos();

        public verProductos()
        {
            InitializeComponent();
        }

        public class Productos
        {
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
        }

        private void verProductos_Load(object sender, EventArgs e)
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

        // Cierra el formulario verProductos
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}