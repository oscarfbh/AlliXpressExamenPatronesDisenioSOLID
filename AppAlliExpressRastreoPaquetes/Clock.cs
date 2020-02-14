using AppAlliExpressRastreoPaquetes.Interfaces;
using System;

namespace AppAlliExpressRastreoPaquetes
{
    public class Clock : IClock
    {
        /// <summary>
        /// Obtiene la fecha y hora actuales.
        /// </summary>
        /// <returns>
        /// Fecha y hora actuales.
        /// </returns>
        public DateTime GetTime()
        {
            return DateTime.Now;
        }
    }
}
