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
    public partial class MenuCliente : Form
    {
        // variable en la cual se almacena el id proporcionado en el login
        private int _id;

        public MenuCliente(int id)
        {
            InitializeComponent();
            _id = id;
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

        private void AbrirFormulario<MiForm>(params object[] parametros) where MiForm : Form
        {
            Form formulario;
            //Busca en la colecion el formulario
            formulario = panelFormulario.Controls.OfType<MiForm>().FirstOrDefault();
            //si el formulario/instancia no existe
            if (formulario == null)
            {
                if (parametros.Length > 0)
                {
                    // Crear instancia con parámetros
                    formulario = (Form)Activator.CreateInstance(typeof(MiForm), parametros);
                }
                else
                {
                    // Crear instancia sin parámetros
                    formulario = (Form)Activator.CreateInstance(typeof(MiForm));
                }
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

        // Abre el formulario actualizarUsuarioCliente en el formulario MenuCliente
        private void btnInicioSesion_Click(object sender, EventArgs e)
        {
            // pasamos el id del usuario recolectado en el login al formulario
            AbrirFormulario<actualizarUsuarioCliente>(_id);
        }

        // Abre el formulario crearPedido en el formulario MenuCliente
        private void button1_Click(object sender, EventArgs e)
        {
            // pasamos el id del usuario recolectado en el login al formulario
            AbrirFormulario<crearPedido>(_id);
        }

        // Abre el formulario actualizarInformacionCliente en el formulario MenuCliente
        private void btnInformacion_Click(object sender, EventArgs e)
        {
            // pasamos el id del usuario recolectado en el login al formulario
            AbrirFormulario<actualizarInformacionCliente>(_id);
        }
    }
}