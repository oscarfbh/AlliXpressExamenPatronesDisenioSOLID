using AppAlliExpressRastreoPaquetes.Interfaces;
using System;

namespace AppAlliExpressRastreoPaquetes.ManejadoresTiempo
{
    class HandlerDias : IHandlerTime
    {
        private IHandlerTime _nextHandlerTime;
        public string CalculaExpresionRangoTiempo(TimeSpan timeSpan)
        {
            string expresion = string.Empty;            
            if (timeSpan.Days > 0)
            {
                expresion = string.Format("{0} {1}", timeSpan.Days, timeSpan.Days.Equals(1) ? "día" : "días");
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
