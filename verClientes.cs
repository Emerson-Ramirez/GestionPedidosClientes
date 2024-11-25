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
    public partial class verClientes : Form
    {
        // instanciando y dejandolo como solo lectura clase Clientes
        private readonly Clientes clientes = new Clientes();

        public verClientes()
        {
            InitializeComponent();
        }

        public class Clientes
        {
            // obtiene todos los registros de la tabla Clientes
            // retorna un objeto DataTable
            public DataTable GetClientes()
            {
                // creando un objeto que permite emular una tabla
                DataTable mydt = new DataTable();
                // instancia de la clase de conexión
                cConexion conectar = new cConexion();
                // asignando la configuración del servidor
                SqlConnection conn = conectar.ConexionServer();
                // declaración de la consulta obtener los datos de la tabla
                string sql = "select idUsuario,nombre,telefono,direccion,departamento,ciudad from Usuarios where tipoUsuario='C';";
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

        private void verClientes_Load(object sender, EventArgs e)
        {
            // creamos el proceso que actualiza el grid
            refrescarGrid(); // llamamos el nombre del proceso
        }

        private void refrescarGrid()
        {
            // completamos el dataGridView1 con el DataTable
            dataGridView1.DataSource = clientes.GetClientes();
        }

        // Cierra el formulario verClientes
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}