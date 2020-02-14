namespace AppAlliExpressRastreoPaquetes.Interfaces
{
    public interface IProcesadorPedidos
    {
        void ImprimirMensajesPedido();

        void SuscribeClienteFabricas(ISuscriberFabricas clienteFabricas);

        void UnSuscribeClienteFabricas(ISuscriberFabricas clienteFabricas);
    }
}
