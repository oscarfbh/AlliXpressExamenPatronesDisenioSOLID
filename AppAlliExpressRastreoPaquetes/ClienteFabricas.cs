using AppAlliExpressRastreoPaquetes.ClasesAuxiliares;
using AppAlliExpressRastreoPaquetes.Interfaces;
using AppAlliExpressRastreoPaquetes.ManejadoresTiempo;
using System;

namespace AppAlliExpressRastreoPaquetes
{
    public class ClienteFabricas : IClientesFabricas, ISuscriberFabricas
    {

        private readonly IEmpresaAbstractFactory _empresa;
        private readonly DateTime _fechaInicioApp;
        private DateTime _fechaPedido;
        private string _medio;
        private string _origen;
        private string _destino;

        public ClienteFabricas(IEmpresaAbstractFactory empresa,
            DateTime fechaPedido,
            string medio,
            string origen,
            string destino,
            DateTime fechaInicioApp)
        {
            _empresa = empresa;
            _fechaPedido = fechaPedido;
            _medio = medio.Trim().ToLower();
            _origen = origen.Trim().ToLower();
            _destino = destino.Trim().ToLower();
            _fechaInicioApp = fechaInicioApp;
        }

        public void AsignarValorFechaPedido(DateTime fecha)
        {
            _fechaPedido = fecha;
        }

        public void AsignarValorMedio(string medio)
        {
            _medio = medio;
        }

        public void AsignarValorOrigen(string origen)
        {
            _origen = origen;
        }

        public void AsignarValorDestino(string destino)
        {
            _destino = destino;
        }

        public EstatusCalculos EfectuarCalculos()
        {
            EstatusCalculos estatusCalculos = new EstatusCalculos();

            switch (_medio)
            {
                case "avion":
                    estatusCalculos = _empresa.CalcularPorAvion();
                    break;
                case "barco":
                    estatusCalculos = _empresa.CalcularPorBarco();
                    break;
                case "tren":
                    estatusCalculos = _empresa.CalcularPorTren();
                    break;
                default:
                    estatusCalculos.Mensaje = string.Format("{0} no ofrece el servicio de transporte {1}, te recomendamos cotizar en otra empresa.", _empresa.RegresarNombreEmpresa(), _medio);
                    estatusCalculos.Color = ConsoleColor.Red;
                    break;
            }

            return estatusCalculos;
        }

        public EstatusCalculos GenerarMensajeCliente()
        {
            EstatusCalculos estatusCalculos = EfectuarCalculos();
            //Si no se encontró detalle en los cálculos procedemos a generar el mensaje del pedido. Sino ya se tiene el mensaje de error.
            if (string.IsNullOrWhiteSpace(estatusCalculos.Mensaje))
            {
                string[] expresiones = GenerarExpresiones(_fechaPedido, estatusCalculos.FechaEntrega, _fechaInicioApp);
                bool pedidoEntregado = DateTime.Compare(_fechaInicioApp, estatusCalculos.FechaEntrega) > 0;

                TimeSpan rangoTiempoAppPedido = pedidoEntregado ? (_fechaInicioApp - estatusCalculos.FechaEntrega) : (estatusCalculos.FechaEntrega - _fechaInicioApp);
                IHandlerTime handlerMeses = new HandlerMeses();
                IHandlerTime handlerDias = new HandlerDias();
                IHandlerTime handlerHoras = new HandlerHoras();
                IHandlerTime handlerMinutos = new HandlerMinutos();
                handlerMeses.SetNext(handlerDias);
                handlerDias.SetNext(handlerHoras);
                handlerHoras.SetNext(handlerMinutos);
                string rangoTiempo = handlerMeses.CalculaExpresionRangoTiempo(rangoTiempoAppPedido);
                estatusCalculos.Color = pedidoEntregado ? ConsoleColor.Green : ConsoleColor.Yellow;

                estatusCalculos.Mensaje =
                    string.Format("Tu paquete {0} de {1} y {2} a {3} {4} {5} y {6} un costo de ${7}. (Cualquier reclamación con {8}).",
                                expresiones[0],
                                _origen,
                                expresiones[1],
                                _destino,
                                expresiones[2],
                                rangoTiempo,
                                expresiones[3],
                                string.Format("{0:0.00}", estatusCalculos.Costo),
                                GetClientName());

            }

            return estatusCalculos;
        }

        private static string[] GenerarExpresiones(DateTime _fechaPedido, DateTime fechaEntrega, DateTime fechaApp)
        {
            string[] expresiones = new string[4];

            expresiones[0] = GenerarExpresion1(_fechaPedido, fechaApp);
            if (DateTime.Compare(fechaEntrega, fechaApp) > 0)
            {
                expresiones[1] = "llegará";
                expresiones[2] = "dentro de";
                expresiones[3] = "tendrá";
            }
            else
            {
                expresiones[1] = "llegó";
                expresiones[2] = "hace";
                expresiones[3] = "tuvo";                
            }

            return expresiones;
        }

        private static string GenerarExpresion1(DateTime fechaInicial, DateTime fechaFinal)
        {
            string expresion = "ha salido";
            if (DateTime.Compare(fechaInicial, fechaFinal) > 0)
            {
                expresion = "salió";
            }

            return expresion;
        }

        public string GetClientName()
        {
            return _empresa.RegresarNombreEmpresa();
        }

        public MensajeMejorCosto CompararCostos(MensajeMejorCosto mensajeMejorCosto)
        {
            MensajeMejorCosto mensajeMejorCostoLocal = mensajeMejorCosto;
            EstatusCalculos estatusCalculos = EfectuarCalculos();

            if (!estatusCalculos.Color.Equals(ConsoleColor.Red))
            {
                double diferenciaCostos = mensajeMejorCosto.Costo - estatusCalculos.Costo;
                if (diferenciaCostos > 0)
                {
                    mensajeMejorCostoLocal.Costo = estatusCalculos.Costo;
                    mensajeMejorCostoLocal.Mensaje = string.Format("Si hubieses pedido en {0} te hubiera costado ${1} más barato.", GetClientName(), string.Format("{0:0.00}", diferenciaCostos));
                }
            }

            return mensajeMejorCostoLocal;
        }

        public IEmpresaAbstractFactory ObtenerEmpresa()
        {
            return _empresa;
        }
    }
}
