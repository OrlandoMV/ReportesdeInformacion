using System;

namespace ReportesdeInformación
{
    public class Juego
    {
        public string Nombre { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int AnoLanzamiento { get; set; }
        public int ReseñasPositivas { get; set; }
        public long EstimadoVentas { get; set; } 

        public Juego() { }

        public override string ToString() => Nombre;
    }
}