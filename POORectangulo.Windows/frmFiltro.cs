using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace POORectangulo.Windows
{
    public partial class frmFiltro : Form
    {
        public frmFiltro()
        {
            InitializeComponent();
        }

        private void frmFiltro_Load(object sender, EventArgs e)
        {

        }

        private int area;
        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                area = int.Parse(AreaTextBox.Text);
                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (!int.TryParse(AreaTextBox.Text, out area))
            {
                valido = false;
                errorProvider1.SetError(AreaTextBox, "Area mal ingresada");
            }
            else if (area<=0)
            {
                valido = false;
                errorProvider1.SetError(AreaTextBox,"El ");
            }

            return valido;
        }

        public int GetArea()
        {
            return area;
        }
    }
}
