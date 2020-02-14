using AppAlliExpressRastreoPaquetes.Interfaces;
using System;

namespace AppAlliExpressRastreoPaquetes.ManejadoresTiempo
{
    public class HandlerMinutos : IHandlerTime
    {
        private IHandlerTime _nextHandlerTime;
        public string CalculaExpresionRangoTiempo(TimeSpan timeSpan)
        {
            string expresion = string.Empty;
            if (timeSpan.Minutes > 0)
            {
                expresion = string.Format("{0} {1}", timeSpan.Minutes, timeSpan.Minutes.Equals(1) ? "minuto" : "minutos");
            }
            else if (_nextHandlerTime != null)
            {
                expresion = _nextHandlerTime.CalculaExpresionRangoTiempo(timeSpan);
            }
            return expresion;
        }

        public void SetNext(IHandlerTime handlerTime)
        {
            _nextHandlerTime = handlerTime;
        }
    }
}
