using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace ReportesdeInformación
{
    partial class Form1
    {
        private DataGridView dataGridView1;
        private Button btnCargar;
        private Button btnBarras;
        private Button btnBarrasRes;
        private Button btnPastelGenero;
        private Button btnPastelFree;
        private Button btnLineJuegosAno;
        private Button btnLinePrecioAno;
        private Chart chart1;
        private Label lblNombreArchivo;
        private Label lblTopVendido;
        private Label lblTopResenas;
        private Label lblTopGenero;
        private Label lblTopAno;
        private Panel panelInferior;
        private FlowLayoutPanel flowButtons;
        private FlowLayoutPanel flowLegend;

        /// <summary>
        /// Required method for Designer support
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            btnCargar = new Button();
            btnBarras = new Button();
            btnBarrasRes = new Button();
            btnPastelGenero = new Button();
            btnPastelFree = new Button();
            btnLineJuegosAno = new Button();
            btnLinePrecioAno = new Button();
            lblNombreArchivo = new Label();
            lblTopVendido = new Label();
            lblTopResenas = new Label();
            lblTopGenero = new Label();
            lblTopAno = new Label();
            panelInferior = new Panel();
            flowButtons = new FlowLayoutPanel();
            flowLegend = new FlowLayoutPanel();
            groupBox1 = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panelInferior.SuspendLayout();
            flowButtons.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeight = 34;
            dataGridView1.Location = new Point(10, 10);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(989, 300);
            dataGridView1.TabIndex = 0;
            // 
            // btnCargar
            // 
            btnCargar.Location = new Point(3, 3);
            btnCargar.Name = "btnCargar";
            btnCargar.Size = new Size(120, 64);
            btnCargar.TabIndex = 0;
            btnCargar.Text = "Cargar CSV";
            btnCargar.Click += btnCargar_Click;
            // 
            // btnBarras
            // 
            btnBarras.Location = new Point(129, 3);
            btnBarras.Name = "btnBarras";
            btnBarras.Size = new Size(140, 64);
            btnBarras.TabIndex = 1;
            btnBarras.Text = "Juegos mas venidos";
            btnBarras.Click += btnBarras_Click;
            // 
            // btnBarrasRes
            // 
            btnBarrasRes.Location = new Point(275, 3);
            btnBarrasRes.Name = "btnBarrasRes";
            btnBarrasRes.Size = new Size(160, 64);
            btnBarrasRes.TabIndex = 2;
            btnBarrasRes.Text = "Juegos con mejores reseñas";
            btnBarrasRes.Click += btnBarrasRes_Click;
            // 
            // btnPastelGenero
            // 
            btnPastelGenero.Location = new Point(441, 3);
            btnPastelGenero.Name = "btnPastelGenero";
            btnPastelGenero.Size = new Size(160, 64);
            btnPastelGenero.TabIndex = 3;
            btnPastelGenero.Text = "Tipo de géneros";
            btnPastelGenero.Click += btnPastelGenero_Click;
            // 
            // btnPastelFree
            // 
            btnPastelFree.Location = new Point(813, 3);
            btnPastelFree.Name = "btnPastelFree";
            btnPastelFree.Size = new Size(195, 64);
            btnPastelFree.TabIndex = 4;
            btnPastelFree.Text = "Porcentaje de juegos gratis y de paga";
            btnPastelFree.Click += btnPastelFree_Click;
            // 
            // btnLineJuegosAno
            // 
            btnLineJuegosAno.Location = new Point(1014, 3);
            btnLineJuegosAno.Name = "btnLineJuegosAno";
            btnLineJuegosAno.Size = new Size(200, 64);
            btnLineJuegosAno.TabIndex = 5;
            btnLineJuegosAno.Text = "Juegos lanzados por Año";
            btnLineJuegosAno.Click += btnLineJuegosAno_Click;
            // 
            // btnLinePrecioAno
            // 
            btnLinePrecioAno.Location = new Point(607, 3);
            btnLinePrecioAno.Name = "btnLinePrecioAno";
            btnLinePrecioAno.Size = new Size(200, 64);
            btnLinePrecioAno.TabIndex = 6;
            btnLinePrecioAno.Text = "Precio promedio por año";
            btnLinePrecioAno.Click += btnLinePrecioAno_Click;
            // 
            // lblNombreArchivo
            // 
            lblNombreArchivo.AutoSize = true;
            lblNombreArchivo.Location = new Point(140, 18);
            lblNombreArchivo.Name = "lblNombreArchivo";
            lblNombreArchivo.Size = new Size(0, 25);
            lblNombreArchivo.TabIndex = 1;
            // 
            // lblTopVendido
            // 
            lblTopVendido.Location = new Point(1005, 34);
            lblTopVendido.Name = "lblTopVendido";
            lblTopVendido.Size = new Size(357, 30);
            lblTopVendido.TabIndex = 1;
            lblTopVendido.Text = "Juego más vendido: -";
            // 
            // lblTopResenas
            // 
            lblTopResenas.Location = new Point(1005, 64);
            lblTopResenas.Name = "lblTopResenas";
            lblTopResenas.Size = new Size(357, 30);
            lblTopResenas.TabIndex = 2;
            lblTopResenas.Text = "Juego con mejores reseñas: -";
            // 
            // lblTopGenero
            // 
            lblTopGenero.Location = new Point(1005, 94);
            lblTopGenero.Name = "lblTopGenero";
            lblTopGenero.Size = new Size(357, 30);
            lblTopGenero.TabIndex = 3;
            lblTopGenero.Text = "Género con más juegos: -";
            // 
            // lblTopAno
            // 
            lblTopAno.Location = new Point(1005, 124);
            lblTopAno.Name = "lblTopAno";
            lblTopAno.Size = new Size(357, 30);
            lblTopAno.TabIndex = 4;
            lblTopAno.Text = "Año con más lanzamientos: -";
            // 
            // panelInferior
            // 
            panelInferior.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelInferior.Controls.Add(flowButtons);
            panelInferior.Location = new Point(10, 320);
            panelInferior.Name = "panelInferior";
            panelInferior.Size = new Size(1471, 370);
            panelInferior.TabIndex = 2;
            // 
            // flowButtons
            // 
            flowButtons.Controls.Add(btnCargar);
            flowButtons.Controls.Add(btnBarras);
            flowButtons.Controls.Add(btnBarrasRes);
            flowButtons.Controls.Add(btnPastelGenero);
            flowButtons.Controls.Add(btnLinePrecioAno);
            flowButtons.Controls.Add(btnPastelFree);
            flowButtons.Controls.Add(btnLineJuegosAno);
            flowButtons.Dock = DockStyle.Top;
            flowButtons.Location = new Point(0, 0);
            flowButtons.Name = "flowButtons";
            flowButtons.Size = new Size(1471, 72);
            flowButtons.TabIndex = 0;
            flowButtons.WrapContents = false;
            // 
            // flowLegend
            // 
            flowLegend.Location = new Point(0, 0);
            flowLegend.Name = "flowLegend";
            flowLegend.Size = new Size(200, 100);
            flowLegend.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Location = new Point(1005, 10);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(476, 300);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "TOP 🏆";
            // 
            // Form1
            // 
            ClientSize = new Size(1493, 700);
            Controls.Add(dataGridView1);
            Controls.Add(lblTopVendido);
            Controls.Add(lblNombreArchivo);
            Controls.Add(lblTopResenas);
            Controls.Add(panelInferior);
            Controls.Add(lblTopGenero);
            Controls.Add(lblTopAno);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Sistema de Análisis de Juegos Steam";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panelInferior.ResumeLayout(false);
            flowButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private GroupBox groupBox1;
    }
}
