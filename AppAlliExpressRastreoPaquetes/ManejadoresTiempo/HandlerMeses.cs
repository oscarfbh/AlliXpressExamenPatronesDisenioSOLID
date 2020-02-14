using AppAlliExpressRastreoPaquetes.Interfaces;
using System;

namespace AppAlliExpressRastreoPaquetes.ManejadoresTiempo
{
    public class HandlerMeses : IHandlerTime
    {
        private IHandlerTime _nextHandlerTime;
        private const int DiasEnMes = 31;
        public string CalculaExpresionRangoTiempo(TimeSpan timeSpan)
        {
            string expresion = string.Empty;
            double meses = Math.Truncate(timeSpan.TotalDays / DiasEnMes);
            if (meses >= 1)
            {
                expresion = string.Format("{0} {1}",meses, meses.Equals(1)?"mes": "meses");
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
