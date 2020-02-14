using AppAlliExpressRastreoPaquetes.Interfaces;
using System;

namespace AppAlliExpressRastreoPaquetes.ManejadoresTiempo
{
    public class HandlerHoras : IHandlerTime
    {
        private IHandlerTime _nextHandlerTime;
        public string CalculaExpresionRangoTiempo(TimeSpan timeSpan)
        {
            string expresion = string.Empty;
            if (timeSpan.Hours > 0)
            {
                expresion = string.Format("{0} {1}", timeSpan.Hours, timeSpan.Hours.Equals(1) ? "hora" : "horas");
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
