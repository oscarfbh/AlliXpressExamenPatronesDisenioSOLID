using System;

namespace AppAlliExpressRastreoPaquetes.Interfaces
{
    public interface IHandlerTime
    {
        void SetNext(IHandlerTime handlerTime);

        string CalculaExpresionRangoTiempo(TimeSpan timeSpan);
    }
}
