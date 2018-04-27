using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Pdfbook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataTable tablita;
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Archivos PdFS|*.pdf";
            openFileDialog1.Multiselect = true;
            tablita.Rows.Clear();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string item in openFileDialog1.FileNames)
                {
                    if (item != null || item != "")
                        tablita.Rows.Add(item);
                }
                dtgconten.DataSource = tablita;
                Analizarpdf();
            }
        }
        public void Analizarpdf()
        {
            int f = 0;
            foreach (DataGridViewRow item in dtgconten.Rows)
            {
                try
                {
                    if (item.Cells["rutaarchivo"].Value != null)
                    {
                        string name = item.Cells["rutaarchivo"].Value.ToString();

                        iTextSharp.text.pdf.PdfReader PdfRreader = new iTextSharp.text.pdf.PdfReader(name);
                        item.Cells["paginas"].Value = PdfRreader.NumberOfPages;
                        item.Cells["Ruta"].Value = name.Substring(0, name.LastIndexOf('\\'));
                        item.Cells["archivo"].Value = name.Substring(name.LastIndexOf('\\') + 1);
                        f += PdfRreader.NumberOfPages;
                    }
                }
                catch (Exception) { }
            }
            lbl.Text = $"Nro Registros {dtgconten.Rows.Count} Nro Páginas {f}";
            dtgconten.Columns["rutaarchivo"].Visible = false;

            dtgconten.Columns["ruta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            tablita = new DataTable();
            tablita.Columns.Add("RutaArchivo", typeof(string));
            tablita.Columns.Add("Ruta", typeof(string));
            tablita.Columns.Add("Archivo", typeof(string));
            tablita.Columns.Add("Paginas", typeof(int));

        }
        public void msg(string g)
        {
            MessageBox.Show(Text, g, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void dtgconten_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            axAcroPDF1.src = (dtgconten["RutaArchivo", e.RowIndex].Value.ToString());
        }
        private void button2_Click(object sender, EventArgs e)
        {
            dtgconten.SelectAll();
            Clipboard.SetDataObject(dtgconten.GetClipboardContent());
        }
        private void dtgconten_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Clipboard.SetDataObject(dtgconten.GetClipboardContent());
        }
        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtruta.Text = folderBrowserDialog1.SelectedPath;
            }

        }
        private void txtruta_TextChanged(object sender, EventArgs e)
        {
            if (txtruta.Text.Length > 0)
                btnbuscar.Enabled = true;
            else
                btnbuscar.Enabled = false;

        }
        public void BuscarDirectorio(string sDir)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    foreach (string f in Directory.GetFiles(d, txtfiltro.Text))
                    {
                        tablita.Rows.Add(f);
                    }
                    BuscarDirectorio(d);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            tablita.Rows.Clear();

            foreach (string f in Directory.GetFiles(txtruta.Text, txtfiltro.Text))
            {
                tablita.Rows.Add(f);
            }

            BuscarDirectorio(txtruta.Text);
            dtgconten.DataSource = tablita;

            Analizarpdf();

            // dtgconten.Columns["rutaarchivo"].Visible = true;

        }
    }
}
