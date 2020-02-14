using AppAlliExpressRastreoPaquetes.ClasesAuxiliares;
using System;

namespace AppAlliExpressRastreoPaquetes.Interfaces
{
    public interface IEmpresaAbstractFactory
    {
        EstatusCalculos CalcularPorBarco();

        EstatusCalculos CalcularPorTren();

        EstatusCalculos CalcularPorAvion();

        string RegresarNombreEmpresa();

        void AsignarValorFechaPedido(DateTime fecha);

        void AsignarValorDistancia(double distancia);
    }
}
