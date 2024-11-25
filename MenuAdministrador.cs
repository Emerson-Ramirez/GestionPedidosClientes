using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionPedidosClientes
{
    public partial class MenuAdministrador : Form
    {
        public MenuAdministrador()
        {
            InitializeComponent();
        }

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
        #endregion

        // Metodo para cerrar sesión/formulario
        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AbrirFormulario<MiForm>() where MiForm : Form, new()
        {
            Form formulario;
            //Busca en la colecion el formulario
            formulario = panelFormularios.Controls.OfType<MiForm>().FirstOrDefault();
            //si el formulario/instancia no existe
            if (formulario == null)
            {
                formulario = new MiForm();
                formulario.TopLevel = false;
                formulario.FormBorderStyle = FormBorderStyle.None;
                //formulario.Dock = DockStyle.Fill;
                panelFormularios.Controls.Add(formulario);
                panelFormularios.Tag = formulario;
                formulario.Show();
                formulario.BringToFront();
            }
            //si el formulario/instancia existe
            else
            {
                formulario.BringToFront();
            }
        }

        // Abre el formulario agregarUsuario en el formulario MenuAdministrador
        private void btnUsuario_Click(object sender, EventArgs e)
        {
            AbrirFormulario<agregarUsuario>();
        }

        // Abre el formulario verProductos en el formulario MenuAdministrador
        private void btnProducto_Click(object sender, EventArgs e)
        {
            AbrirFormulario<verProductos>();
        }

        // Abre el formulario verClientes en el formulario MenuAdministrador
        private void btnCliente_Click(object sender, EventArgs e)
        {
            AbrirFormulario<verClientes>();
        }

        // Abre el formulario pedidosFiltros en el formulario MenuAdministrador
        private void btnPedido_Click(object sender, EventArgs e)
        {
            AbrirFormulario<pedidosFiltros>();
        }
    }
}