using System;

namespace AppAlliExpressRastreoPaquetes.Interfaces
{
    public interface IProduct
    {
        double CalcularTiempoTrasladoHoras(double distancia);

        DateTime CalcularFechaEntrega(DateTime fechaPedido, double tiempoTrasladoHr);

        double CalcularCosto(double costoPorKm, double distancia, double margenUtilidad);

    }
}
