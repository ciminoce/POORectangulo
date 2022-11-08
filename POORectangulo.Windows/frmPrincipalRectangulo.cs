using POORectángulo.Datos;
using POORectángulo.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace POORectangulo.Windows
{
    public partial class frmPrincipalRectangulo : Form
    {   
        public frmPrincipalRectangulo()
        {
            InitializeComponent();
        }
        private List<Rectangulo> lista;
        private RepositorioDeRectangulos repo;
        private int cantidad;

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void frmPrincipalRectangulo_Load(object sender, EventArgs e)
        {
            repo = new RepositorioDeRectangulos();
            lista = repo.GetLista();
            cantidad = repo.GatCantidad();
            if (cantidad > 0)
            {
                MostrarLista();
            }
            else
            {
                MessageBox.Show("El Repositorio esta vacío", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            MostrarTotal();
        }

        private void MostrarLista()
        {
            RectanguloDataGridView.Rows.Clear();
            foreach (var Rectangulo in lista)
            {
                var r = ConstruirFila();
                SetearFila(r, Rectangulo);
                AgregarFila(r);
            }
        }

        private void AgregarFila(DataGridViewRow r)
        {
            RectanguloDataGridView.Rows.Add(r);
        }

        private void SetearFila(DataGridViewRow r, Rectangulo rectangulo)
        {
            r.Cells[colRectanguloAlto.Index].Value = rectangulo.Alto;
            r.Cells[colRectanguloBase.Index].Value = rectangulo.Largo;
            r.Cells[colArea.Index].Value = rectangulo.GetArea().ToString();
            r.Cells[colPerimetro.Index].Value = rectangulo.GetPerimetro().ToString();
            

            //Faltó la property tag
            r.Tag = rectangulo;
        }

        private DataGridViewRow ConstruirFila()
        {
            var r = new DataGridViewRow ();
            r.CreateCells(RectanguloDataGridView);
            return r;
        }

        private void MostrarTotal()
        {
            CantidadLabel.Text = cantidad.ToString();
        }

        private void RectanguloDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            MostrarLista();
        }

        private void NuevoToolStripButton_Click(object sender, EventArgs e)
        {
            frmNuevoRectangulo frm = new frmNuevoRectangulo() { Text = "Agregar nuevo rectangulo" };
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Cancel)
            {
                MessageBox.Show("El usuario canceló");
                return;
            }
            var RectNuevo = frm.GetRectangulo();
            repo.Agregar(RectNuevo);
            var r = ConstruirFila();
            SetearFila(r, RectNuevo);
            AgregarFila(r);
            MessageBox.Show("Rectangulo agregado");
            cantidad = repo.GatCantidad();
            MostrarTotal();
        }

        private void SalirToolStripButton_Click(object sender, EventArgs e)
        {
            //if (HayCambios)
            //{
            //    ManejadorArchivosSecuencial.GuardarEnArchivosSecuencial(Lista);
            //}
            repo.Guardar();

            DialogResult dr = MessageBox.Show("¿Desea salir del programa?", "Confirmar salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.No)
            {
                return;
            }
            Application.Exit();
        }

        private void toolStrip1_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void BorrarToolStripButton_Click(object sender, EventArgs e)
        {
            //Veo si tengo seleccionada alguna fila
            if (RectanguloDataGridView.SelectedRows.Count == 0)
            {
                return;
            }
            //Si seleccione alguna fila
            var r = RectanguloDataGridView.SelectedRows[0];
            Rectangulo rect = r.Tag as Rectangulo;
            DialogResult dr = MessageBox.Show("¿Desea eliminar la fila?", "Confirmar selección", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.No)
            {
                MessageBox.Show("El usuario se arreointió");
                return;
            }

            if (repo.Borrar(rect))
            {
               repo.Borrar(rect);//El repo borra el Rect
               lista.Remove(rect);//La saco de la lista de memoria
               RectanguloDataGridView.Rows.Remove(r);//La saco de la Grilla
               cantidad = repo.GatCantidad();
               MostrarTotal();
               MessageBox.Show("Rectangulo eliminado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("No se pudo dar de baja", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
                
            
           
        }

        private void RestaurarToolStripButton_Click(object sender, EventArgs e)
        {
            lista = repo.GetLista();
            MostrarLista();
            cantidad = repo.GatCantidad();
            MostrarTotal();
        }

        private void EditarToolStripButton_Click(object sender, EventArgs e)
        {
            if (RectanguloDataGridView.SelectedRows.Count == 0)
            {
                return;
            }
            var r = RectanguloDataGridView.SelectedRows[0];
            Rectangulo rect = r.Tag as Rectangulo;
            frmNuevoRectangulo frm = new frmNuevoRectangulo() { Text = "Editar Rectángulo" };
            frm.SetRectangulo(rect);
            DialogResult dr=frm.ShowDialog(this);
            if (dr == DialogResult.Cancel)
            {
                return;
            }
            rect = frm.GetRectangulo();
            SetearFila(r, rect);
            repo.Editar(rect);
            MessageBox.Show("Rectangulo modificado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void FiltrarToolStripButton_Click(object sender, EventArgs e)
        {
            frmFiltro frm = new frmFiltro() { Text = "Datos para el filtro" };
            DialogResult dr = frm.ShowDialog(this);
            if (dr==DialogResult.Cancel)
            {
                return;
            }

            var areaFiltrar = frm.GetArea();
            lista = repo.FiltrarDatos(areaFiltrar);
            MostrarLista();
            cantidad = repo.GatCantidadFiltrada(areaFiltrar);
            MostrarTotal();
        }

        private void GuardarToolStripButton_Click(object sender, EventArgs e)
        {
            ManejadorArchivosSecuencial.GuardarEnArchivosSecuencial(repo.GetLista());
            MessageBox.Show("Registros Guardados", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void ascendentePorAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var orden = Orden.Ascendente;
            lista = repo.Ordenar(orden);
            MostrarLista();

        }

        private void descendentePorAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var orden = Orden.Descendente;
            lista = repo.Ordenar(orden);
            MostrarLista();
        }
    }
}
