using AppAlliExpressRastreoPaquetes.ClasesAuxiliares;
using AppAlliExpressRastreoPaquetes.Interfaces;
using System;
using System.Collections.Generic;

namespace AppAlliExpressRastreoPaquetes
{
    public class ProcesadorPedidos: IProcesadorPedidos
    {
        private readonly IClientesFabricas _clienteFabricasPedido;
        private readonly List<ISuscriberFabricas> _clientesFabricas;
        

        public ProcesadorPedidos(IClientesFabricas clienteFabricas)
        {
            _clienteFabricasPedido = clienteFabricas;
            _clientesFabricas = new List<ISuscriberFabricas>();
        }

        //Por tiempo no se genera una clase que imprima.
        public void ImprimirMensajesPedido()
        {
            EstatusCalculos estatusCalculosPedido = _clienteFabricasPedido.GenerarMensajeCliente();

            Console.ForegroundColor = estatusCalculosPedido.Color;
            Console.WriteLine(estatusCalculosPedido.Mensaje);
            if (!estatusCalculosPedido.Color.Equals(ConsoleColor.Red))
            {
                MensajeMejorCosto mensajeMejorCosto = new MensajeMejorCosto() {
                    Costo = estatusCalculosPedido.Costo,
                    Mensaje = string.Empty
                };
                foreach (ISuscriberFabricas cliente in _clientesFabricas)
                {
                    mensajeMejorCosto = cliente.CompararCostos(mensajeMejorCosto);
                }

                if (!string.IsNullOrWhiteSpace(mensajeMejorCosto.Mensaje))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(mensajeMejorCosto.Mensaje);
                }
             }

            Console.WriteLine();
        }

        public void SuscribeClienteFabricas(ISuscriberFabricas clienteFabricas)
        {
            if (!_clientesFabricas.Contains(clienteFabricas))
            {
                _clientesFabricas.Add(clienteFabricas);
            }
        }

        public void UnSuscribeClienteFabricas(ISuscriberFabricas clienteFabricas)
        {
            if (_clientesFabricas.Contains(clienteFabricas))
            {
                _clientesFabricas.Remove(clienteFabricas);
            }
        }
    }
}
