using System;

namespace AppAlliExpressRastreoPaquetes.ClasesAuxiliares
{
    public class EstatusCalculos
    {
        public DateTime FechaEntrega { get; set; }

        public string Mensaje { get; set; }

        public ConsoleColor Color { get; set; } = ConsoleColor.Green;

        public double Costo { get; set; }
    }
}
