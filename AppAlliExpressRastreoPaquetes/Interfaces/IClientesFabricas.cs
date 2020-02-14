using AppAlliExpressRastreoPaquetes.ClasesAuxiliares;
using System;

namespace AppAlliExpressRastreoPaquetes.Interfaces
{
    public interface IClientesFabricas
    {
        EstatusCalculos EfectuarCalculos();

        EstatusCalculos GenerarMensajeCliente();

        string GetClientName();

        void AsignarValorFechaPedido(DateTime fecha);

        void AsignarValorMedio(string medio);

        void AsignarValorOrigen(string origen);

        void AsignarValorDestino(string destino);

        IEmpresaAbstractFactory ObtenerEmpresa();
    }
}
