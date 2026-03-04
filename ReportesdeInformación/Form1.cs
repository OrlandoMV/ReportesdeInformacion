using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace ReportesdeInformación
{
    public partial class Form1 : Form
    {
        private BindingList<Juego> juegos = new BindingList<Juego>();


        private System.Drawing.Color ColorFromIndex(int index)
        {

            double hue = (index * 37) % 360;
            double saturation = 0.6;
            double lightness = 0.55;
            return HslToRgb(hue, saturation, lightness);
        }

        private System.Drawing.Color HslToRgb(double h, double s, double l)
        {
            h = h / 360.0;
            double r = 0, g = 0, b = 0;
            if (s == 0)
            {
                r = g = b = l;
            }
            else
            {
                Func<double, double, double, double> hue2rgb = (p, q, t) =>
                {
                    if (t < 0) t += 1;
                    if (t > 1) t -= 1;
                    if (t < 1.0 / 6) return p + (q - p) * 6 * t;
                    if (t < 1.0 / 2) return q;
                    if (t < 2.0 / 3) return p + (q - p) * (2.0 / 3 - t) * 6;
                    return p;
                };
                double q = l < 0.5 ? l * (1 + s) : l + s - l * s;
                double p = 2 * l - q;
                r = hue2rgb(p, q, h + 1.0 / 3);
                g = hue2rgb(p, q, h);
                b = hue2rgb(p, q, h - 1.0 / 3);
            }
            return System.Drawing.Color.FromArgb(
                (int)Math.Round(r * 255),
                (int)Math.Round(g * 255),
                (int)Math.Round(b * 255));
        }

        private void MostrarLeyenda(List<(string label, int count)> items)
        {
            if (flowLegend == null)
                return;
            flowLegend.SuspendLayout();
            flowLegend.Controls.Clear();
            flowLegend.AutoSize = true;
            flowLegend.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            int total = items.Sum(x => x.count);
            if (items.Count == 0)
            {
                flowLegend.ResumeLayout();
                return;
            }

            int cols = items.Count > 30 ? 4 : (items.Count > 15 ? 3 : (items.Count > 8 ? 2 : 1));
            int rows = (int)Math.Ceiling(items.Count / (double)cols);

            var table = new TableLayoutPanel
            {
                ColumnCount = cols,
                RowCount = rows,
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(6),
                Padding = new Padding(0)
            };

            for (int c = 0; c < cols; c++)
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / cols));
            for (int r = 0; r < rows; r++)
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            int idx = 0;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (idx >= items.Count) break;
                    var it = items[idx];

                    var itemFlow = new FlowLayoutPanel
                    {
                        AutoSize = true,
                        FlowDirection = FlowDirection.LeftToRight,
                        WrapContents = false,
                        Margin = new Padding(3)
                    };

                    var colorPanel = new Panel
                    {
                        Size = new System.Drawing.Size(18, 14),
                        BackColor = ColorFromIndex(idx),
                        Margin = new Padding(3, 6, 6, 3)
                    };

                    var lbl = new Label
                    {
                        AutoSize = true,
                        Text = total > 0 ? $"{it.label} ({it.count}) - {((double)it.count / total):P1}" : $"{it.label} ({it.count})",
                        Margin = new Padding(0, 3, 12, 3)
                    };

                    itemFlow.Controls.Add(colorPanel);
                    itemFlow.Controls.Add(lbl);
                    table.Controls.Add(itemFlow, c, r);
                    idx++;
                }
            }

            flowLegend.Controls.Add(table);
            flowLegend.ResumeLayout();
        }

        public Form1()
        {
            InitializeComponent();
            // usar las columnas definidas en el diseñador
            // (InitializeComponent ya establece AutoGenerateColumns = false)
            dataGridView1.DataSource = juegos;
            Text = "Sistema de Análisis de Juegos Steam";
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            CargarCsvYMostrar();
        }

        private void btnBarras_Click(object sender, EventArgs e)
        {
            GenerarGraficaBarrasTopVentas();
        }

        private void btnBarrasRes_Click(object sender, EventArgs e)
        {
            GenerarGraficaBarrasTopResenas();
        }

        private void btnPastelGenero_Click(object sender, EventArgs e)
        {
            GenerarGraficaPastelGenero();
        }

        private void btnPastelFree_Click(object sender, EventArgs e)
        {
            GenerarGraficaPastelFreeVsPaid();
        }

        private void btnLineJuegosAno_Click(object sender, EventArgs e)
        {
            GenerarGraficaLineJuegosPorAno();
        }

        private void btnLinePrecioAno_Click(object sender, EventArgs e)
        {
            GenerarGraficaLinePrecioPromedioPorAno();
        }


        private void CargarCsvYMostrar()
        {
            using OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Seleccionar archivo steam.csv",
                CheckFileExists = true,
                Multiselect = false
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            lblNombreArchivo.Text = Path.GetFileName(ofd.FileName);

            try
            {
                var lista = LeerCsvComoJuegos(ofd.FileName);
                juegos = new BindingList<Juego>(lista);
                dataGridView1.DataSource = juegos;

                ActualizarIndicadores();
                MessageBox.Show($"Se cargaron {juegos.Count} registros.", "Carga completa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar CSV: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<Juego> LeerCsvComoJuegos(string path)
        {
            List<Juego> resultado = new List<Juego>();

            using TextFieldParser parser = new TextFieldParser(path)
            {
                TextFieldType = FieldType.Delimited
            };
            parser.SetDelimiters(",");

            if (parser.EndOfData)
                return resultado;

            string[] headers = parser.ReadFields();
            if (headers == null)
                return resultado;

            int idxNombre = BuscarIndice(headers, "name", "nombre", "title");
            int idxGenero = BuscarIndice(headers, "genres", "genre", "genero", "tags", "steamspy_tags");
            int idxPrecio = BuscarIndice(headers, "price", "precio");
            int idxRelease = BuscarIndice(headers, "release_date", "release", "released", "year");
            int idxResenasPos = BuscarIndice(headers, "positive_ratings", "positive", "positive_reviews", "pos_reviews");
            int idxOwners = BuscarIndice(headers, "owners", "estimated_owners", "owners_estimate", "sale_estimate", "popularity", "ccu");

            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields() ?? Array.Empty<string>();

                try
                {
                    Juego j = new Juego
                    {
                        Nombre = ObtenerCampo(fields, idxNombre),
                        Genero = NormalizarGenero(ObtenerCampo(fields, idxGenero)),
                        Precio = ParsearDecimalSeguro(ObtenerCampo(fields, idxPrecio)),
                        AnoLanzamiento = ParsearAnoDesdeCampo(ObtenerCampo(fields, idxRelease)),
                        ReseñasPositivas = ParsearIntSeguro(ObtenerCampo(fields, idxResenasPos)),
                        EstimadoVentas = ParsearLongSeguro(ObtenerCampo(fields, idxOwners))
                    };

                    resultado.Add(j);
                }
                catch
                {
                    continue;
                }
            }

            return resultado;
        }

        private int BuscarIndice(string[] headers, params string[] posiblesNombres)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                string h = headers[i]?.Trim().ToLowerInvariant() ?? "";
                foreach (var nombre in posiblesNombres)
                {
                    if (string.IsNullOrWhiteSpace(nombre)) continue;
                    if (h == nombre.ToLowerInvariant() || h.Contains(nombre.ToLowerInvariant()))
                        return i;
                }
            }
            return -1;
        }

        private string ObtenerCampo(string[] fields, int idx)
        {
            if (idx < 0 || idx >= fields.Length)
                return string.Empty;
            return fields[idx]?.Trim() ?? string.Empty;
        }

        private string NormalizarGenero(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return "Desconocido";
            var sep = raw.Contains(";") ? ';' : (raw.Contains(",") ? ',' : '|');
            var first = raw.Split(sep)[0];
            return first.Trim();
        }

        private decimal ParsearDecimalSeguro(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0m;
            s = s.Replace("$", "").Replace("€", "").Trim();
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
                return v;
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out v))
                return v;
            return 0m;
        }

        private int ParsearIntSeguro(string s)
        {
            if (int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
                return v;
            if (int.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out v))
                return v;
            return 0;
        }

        private long ParsearLongSeguro(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;

            string cleaned = new string(s.Where(c => char.IsDigit(c)).ToArray());
            if (long.TryParse(cleaned, out var v))
                return v;
            return 0;
        }

        private int ParsearAnoDesdeCampo(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;

            foreach (var token in s.Split(new[] { ' ', '/', '-', ',', '.' }, StringSplitOptions.RemoveEmptyEntries).Reverse())
            {
                if (token.Length == 4 && int.TryParse(token, out int y) && y > 1900 && y <= DateTime.Now.Year)
                    return y;
            }

            if (DateTime.TryParse(s, out DateTime dt))
                return dt.Year;
            return 0;
        }

        private void PrepararChart(string titulo)
        {
            if (chart1 == null)
            {
                chart1 = new Chart();
                chart1.Dock = DockStyle.Fill;
                if (panelInferior != null && !panelInferior.Controls.Contains(chart1))
                    panelInferior.Controls.Add(chart1);
            }

            chart1.Series.Clear();
            chart1.Titles.Clear();
            chart1.ChartAreas.Clear();
            chart1.Legends.Clear();

            chart1.Titles.Add(titulo);
            var area = new ChartArea("Area1");
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = -45;
            chart1.ChartAreas.Add(area);
        }

        private void GenerarGraficaBarrasTopVentas()
        {
            try
            {
                if (!juegos.Any())
                {
                    MessageBox.Show("Cargue primero los datos.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                PrepararChart("Top 10 - Juegos más vendidos");
                var data = juegos.OrderByDescending(j => j.EstimadoVentas).Where(j => j.EstimadoVentas > 0).Take(10).ToList();
                var s = new Series("Ventas")
                {
                    ChartType = SeriesChartType.Bar,
                    IsValueShownAsLabel = true
                };
                foreach (var j in data)
                {
                    s.Points.AddXY(j.Nombre, j.EstimadoVentas);
                }
                chart1.Series.Add(s);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar gráfica (Top ventas): {ex.Message}\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void GenerarGraficaBarrasTopResenas()
        {
            try
            {
                if (!juegos.Any())
                {
                    MessageBox.Show("Cargue primero los datos.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                PrepararChart("Top 10 - Juegos con mejores reseñas (positivas)");
                var data = juegos.OrderByDescending(j => j.ReseñasPositivas).Where(j => j.ReseñasPositivas > 0).Take(10).ToList();
                var s = new Series("Reseñas")
                {
                    ChartType = SeriesChartType.Bar,
                    IsValueShownAsLabel = true
                };
                foreach (var j in data)
                {
                    s.Points.AddXY(j.Nombre, j.ReseñasPositivas);
                }
                chart1.Series.Add(s);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar gráfica (Top reseñas): {ex.Message}\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void GenerarGraficaPastelGenero()
        {
            try
            {
                if (!juegos.Any())
                {
                    MessageBox.Show("Cargue primero los datos.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                PrepararChart("Distribución de juegos por género");
                var grupos = juegos.GroupBy(j => string.IsNullOrWhiteSpace(j.Genero) ? "Desconocido" : j.Genero)
                                   .Select(g => new { Genero = g.Key, Count = g.Count() })
                                   .OrderByDescending(x => x.Count)
                                   .ToList();

                var s = new Series("Generos")
                {
                    ChartType = SeriesChartType.Pie,
                    IsValueShownAsLabel = false,
                    LabelFormat = "#PERCENT{P1}"
                };
                foreach (var g in grupos)
                {
                    var dp = new DataPoint();
                    dp.YValues = new double[] { g.Count };
                    dp.LegendText = g.Genero;
                    // mostrar porcentaje junto al conteo
                    dp.Label = $"{g.Genero} ({g.Count}) - #PERCENT{{P1}}";
                    s.Points.Add(dp);
                }

                chart1.Series.Add(s);
                chart1.Legends.Clear();
                chart1.Legends.Add(new Legend("L") { Docking = Docking.Bottom });

                // Mostrar leyenda detallada fuera del gráfico
                MostrarLeyenda(grupos.Select(x => (label: x.Genero, count: x.Count)).ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar gráfica (Géneros): {ex.Message}\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void GenerarGraficaPastelFreeVsPaid()
        {
            try
            {
                if (!juegos.Any())
                {
                    MessageBox.Show("Cargue primero los datos.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                PrepararChart("Porcentaje Juegos Gratuitos vs De Paga");
                var grupos = juegos.GroupBy(j => j.Precio == 0m ? "Gratuito" : "De paga")
                                   .Select(g => new { Key = g.Key, Count = g.Count() }).ToList();

                var s = new Series("FreePaid")
                {
                    ChartType = SeriesChartType.Pie,
                    IsValueShownAsLabel = false,
                    LabelFormat = "#PERCENT{P1}"
                };
                foreach (var g in grupos)
                {
                    var dp = new DataPoint();
                    dp.YValues = new double[] { g.Count };
                    dp.LegendText = g.Key;
                    dp.Label = $"{g.Key} ({g.Count}) - #PERCENT{{P1}}";
                    s.Points.Add(dp);
                }
                chart1.Series.Add(s);
                chart1.Legends.Clear();
                chart1.Legends.Add(new Legend("L") { Docking = Docking.Bottom });

                MostrarLeyenda(grupos.Select(x => (label: x.Key, count: x.Count)).ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar gráfica (Free vs Paid): {ex.Message}\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerarGraficaLineJuegosPorAno()
        {
            try
            {
                if (!juegos.Any())
                {
                    MessageBox.Show("Cargue primero los datos.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                PrepararChart("Cantidad de juegos publicados por año");
                var data = juegos.Where(j => j.AnoLanzamiento > 0)
                                 .GroupBy(j => j.AnoLanzamiento)
                                 .Select(g => new { Ano = g.Key, Count = g.Count() })
                                 .OrderBy(x => x.Ano)
                                 .ToList();

                var s = new Series("JuegosPorAño")
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    XValueType = ChartValueType.Int32,
                    IsValueShownAsLabel = true
                };
                foreach (var p in data)
                {
                    s.Points.AddXY(p.Ano, p.Count);
                }
                if (chart1.ChartAreas.Count > 0)
                    chart1.ChartAreas[0].AxisX.Interval = 1;
                chart1.Series.Add(s);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar gráfica (Juegos por año): {ex.Message}\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerarGraficaLinePrecioPromedioPorAno()
        {
            try
            {
                if (!juegos.Any())
                {
                    MessageBox.Show("Cargue primero los datos.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                PrepararChart("Evolución del precio promedio por año");
                var data = juegos.Where(j => j.AnoLanzamiento > 0)
                                 .GroupBy(j => j.AnoLanzamiento)
                                 .Select(g => new { Ano = g.Key, AvgPrice = g.Average(x => (double)x.Precio) })
                                 .OrderBy(x => x.Ano)
                                 .ToList();

                var s = new Series("PrecioPromedio")
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    XValueType = ChartValueType.Int32,
                    IsValueShownAsLabel = true
                };
                foreach (var p in data)
                {
                    s.Points.AddXY(p.Ano, Math.Round(p.AvgPrice, 2));
                }
                if (chart1.ChartAreas.Count > 0)
                    chart1.ChartAreas[0].AxisX.Interval = 1;
                chart1.Series.Add(s);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar gráfica (Precio promedio por año): {ex.Message}\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarIndicadores()
        {
            if (!juegos.Any())
            {
                lblTopVendido.Text = "Juego más vendido: -";
                lblTopResenas.Text = "Juego con mejores reseñas: -";
                lblTopGenero.Text = "Género con más juegos: -";
                lblTopAno.Text = "Año con más lanzamientos: -";
                return;
            }

            var topVendido = juegos.OrderByDescending(j => j.EstimadoVentas).FirstOrDefault();
            var topRes = juegos.OrderByDescending(j => j.ReseñasPositivas).FirstOrDefault();
            var topGenero = juegos.GroupBy(j => j.Genero)
                                  .OrderByDescending(g => g.Count())
                                  .FirstOrDefault()?.Key ?? "Desconocido";
            var topAno = juegos.Where(j => j.AnoLanzamiento > 0)
                               .GroupBy(j => j.AnoLanzamiento)
                               .OrderByDescending(g => g.Count())
                               .FirstOrDefault()?.Key ?? 0;

            lblTopVendido.Text = topVendido != null ? "Juego más vendido: " + topVendido.Nombre + " (" + topVendido.EstimadoVentas + ")" : "Juego más vendido: -";
            lblTopResenas.Text = topRes != null ? "Juego con mejores reseñas: " + topRes.Nombre + " (" + topRes.ReseñasPositivas + ")" : "Juego con mejores reseñas: -";
            lblTopGenero.Text = $"Género con más juegos: {topGenero}";
            lblTopAno.Text = $"Año con más lanzamientos: {topAno}";
        }

    }
}
