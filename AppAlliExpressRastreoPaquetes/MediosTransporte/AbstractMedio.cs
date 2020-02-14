using AppAlliExpressRastreoPaquetes.Interfaces;
using System;

namespace AppAlliExpressRastreoPaquetes.MediosTransporte
{
    public abstract class AbstractMedio : IProduct
    {
        public double CostoPorKilometro { get; set; }
        public double VelocidadEntregaKmH { get; set; }

        public virtual double CalcularTiempoTrasladoHoras(double distancia)
        {
            return distancia / VelocidadEntregaKmH;
        }


        public virtual double CalcularCosto(double costoPorKm, double distancia, double margenUtilidad)
        {
            return costoPorKm * distancia * (1 + (margenUtilidad / 100));
        }

        public virtual DateTime CalcularFechaEntrega(DateTime fechaPedido, double tiempoTrasladoHr)
        {
            TimeSpan intervalo = TimeSpan.FromHours(tiempoTrasladoHr);
            DateTime fechaPedidoAux = fechaPedido;
            fechaPedidoAux.AddDays(intervalo.Days);
            fechaPedidoAux.AddHours(intervalo.Hours);
            fechaPedidoAux.AddMinutes(intervalo.Minutes);
            fechaPedidoAux.AddSeconds(intervalo.Seconds);

            return fechaPedidoAux;
        }
    }
}
