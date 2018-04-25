using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            dtgconten.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        public void Analizarpdf()
        {
            int f = 0;
            foreach (DataGridViewRow item in dtgconten.Rows)
            {
                if (item.Cells["rutaarchivo"].Value != null)
                {
                    //axAcroPDF1.src = item.Cells["rutaarchivo"].Value.ToString();
                    //axAcroPDF1.gotoLastPage();
                    string name = item.Cells["rutaarchivo"].Value.ToString();
                    axAcroPDF1.src = name;
                    iTextSharp.text.pdf.PdfReader PdfRreader = new iTextSharp.text.pdf.PdfReader(name);

                    item.Cells["paginas"].Value = PdfRreader.NumberOfPages;



                    item.Cells["archivo"].Value = name.Substring(name.LastIndexOf('\\') + 1);
                    f += PdfRreader.NumberOfPages;
                }
            }
            lbl.Text = $"Nro Registros {dtgconten.Rows.Count} Nro Páginas {f}";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            tablita = new DataTable();
            tablita.Columns.Add("RutaArchivo", typeof(string));
            tablita.Columns.Add("Archivo", typeof(string));
            tablita.Columns.Add("Paginas", typeof(int));
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
    }
}
