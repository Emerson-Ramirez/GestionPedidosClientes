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
    public partial class MenuOperador : Form
    {
        public MenuOperador()
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
            formulario = panelFormulario.Controls.OfType<MiForm>().FirstOrDefault();
            //si el formulario/instancia no existe
            if (formulario == null)
            {
                formulario = new MiForm();
                formulario.TopLevel = false;
                formulario.FormBorderStyle = FormBorderStyle.None;
                //formulario.Dock = DockStyle.Fill;
                panelFormulario.Controls.Add(formulario);
                panelFormulario.Tag = formulario;
                formulario.Show();
                formulario.BringToFront();
            }
            //si el formulario/instancia existe
            else
            {
                formulario.BringToFront();
            }
        }

        // Abre el formulario agregarProducto en el formulario MenuOperador
        private void btnProducto_Click(object sender, EventArgs e)
        {
            AbrirFormulario<agregarProducto>();
        }
    }
}