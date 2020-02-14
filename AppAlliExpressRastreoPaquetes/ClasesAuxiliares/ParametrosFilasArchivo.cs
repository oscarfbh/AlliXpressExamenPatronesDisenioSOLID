using System;

namespace AppAlliExpressRastreoPaquetes.ClasesAuxiliares
{
    public class ParametrosFilasArchivo
    {
        public string Origen { get; set; }
        public string Destino { get; set; }

        public double Distancia { get; set; }

        public string Paqueteria { get; set; }

        public string MedioTransporte { get; set; }

        public DateTime FechaPedido { get; set; }
    }
}
