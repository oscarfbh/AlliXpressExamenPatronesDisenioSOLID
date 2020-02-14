using AppAlliExpressRastreoPaquetes.ClasesAuxiliares;
using AppAlliExpressRastreoPaquetes.Empresas;
using AppAlliExpressRastreoPaquetes.Interfaces;
using System;
using System.Collections.Generic;

namespace AppAlliExpressRastreoPaquetes
{
    public class ProcesadorArchivoPedidos
    {
        private readonly string _path;
        private readonly string _fileName;
        private readonly IFileExistValidator _fileExistValidator;
        private readonly IFileDataReader _fileDataReader;
        private readonly Dictionary<string, IClientesFabricas> _clientesFabricas;
        private readonly Dictionary<string, IProcesadorPedidos> _procesadoresPedidos;
        private readonly DateTime _tiempoApp;

        public ProcesadorArchivoPedidos(string path,
            string fileName,
            IFileExistValidator fileExistValidator,
            IFileDataReader fileDataReader,
            IClock clock)
        {
            _path = path;
            _fileName = fileName;
            _fileExistValidator = fileExistValidator;
            _fileDataReader = fileDataReader;
            _clientesFabricas = new Dictionary<string, IClientesFabricas>();
            _procesadoresPedidos = new Dictionary<string, IProcesadorPedidos>();
            _tiempoApp = clock.GetTime();
        }

        public void ProcesarArchivo()
        {
            try
            {
                string mensajeError = _fileExistValidator.ValidateFileExist(_path, _fileName);
                if (string.IsNullOrEmpty(mensajeError))
                {
                    string[] pedidos = _fileDataReader.GetFileDataRows(_path, _fileName);
                    GenerarProcesadoresPedidos();
                    ParametrosFilasArchivo parametrosFilas;

                    foreach (string pedido in pedidos)
                    {
                        try
                        {
                            parametrosFilas = ObtenerParametrosPedido(pedido);
                            if (_procesadoresPedidos.ContainsKey(parametrosFilas.Paqueteria))
                            {
                                ActualizarParametros(parametrosFilas);
                                LlamarImprimirMensajesPedidoProcesadorPedido(_procesadoresPedidos[parametrosFilas.Paqueteria]);
                            }
                            else
                            {
                                LlamarMetodoConsoleMethods(string.Format("La paquetería {0} no se encuentra registrada en nuestra red de distribución.", parametrosFilas.Paqueteria));
                            }
                        }
                        catch (Exception e)
                        {
                            LlamarMetodoConsoleMethods(e.Message);
                        }
                    }
                }
                else
                {
                    LlamarMetodoConsoleMethods(mensajeError);
                }
            }
            catch (Exception e)
            {
                LlamarMetodoConsoleMethods(e.Message);
            }
        }

        protected virtual bool LlamarImprimirMensajesPedidoProcesadorPedido(IProcesadorPedidos procesadorPedidos)
        {
            procesadorPedidos.ImprimirMensajesPedido();
            return true;
        }

        protected virtual bool LlamarMetodoConsoleMethods(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(mensaje);
            Console.ResetColor();
            Console.WriteLine();
            return true;
        }

        private void ActualizarParametros(ParametrosFilasArchivo parametros)
        {
            foreach (KeyValuePair<string, IClientesFabricas> clienteFabrica in _clientesFabricas)
            {
                clienteFabrica.Value.AsignarValorOrigen(parametros.Origen);
                clienteFabrica.Value.AsignarValorDestino(parametros.Destino);
                clienteFabrica.Value.AsignarValorMedio(parametros.MedioTransporte);
                clienteFabrica.Value.AsignarValorFechaPedido(parametros.FechaPedido);
                IEmpresaAbstractFactory empresa = clienteFabrica.Value.ObtenerEmpresa();
                empresa.AsignarValorDistancia(parametros.Distancia);
                empresa.AsignarValorFechaPedido(parametros.FechaPedido);
            }
        }

        private void GenerarClientesFabricas()
        {
            IEmpresaAbstractFactory empresaDhl = new DhlFactory(_tiempoApp, 0);
            IEmpresaAbstractFactory empresaEstafeta = new EstafetaFactory(_tiempoApp, 0);
            IEmpresaAbstractFactory empresaFedex = new FedexFactory(_tiempoApp, 0);

            IClientesFabricas clientesFabricaDhl = new ClienteFabricas(empresaDhl, _tiempoApp, "", "", "", _tiempoApp);
            IClientesFabricas clientesFabricaEstafeta = new ClienteFabricas(empresaEstafeta, _tiempoApp, "", "", "", _tiempoApp);
            IClientesFabricas clientesFabricaFedex = new ClienteFabricas(empresaFedex, _tiempoApp, "", "", "", _tiempoApp);

            _clientesFabricas.Add("dhl", clientesFabricaDhl);
            _clientesFabricas.Add("estafeta", clientesFabricaEstafeta);
            _clientesFabricas.Add("fedex", clientesFabricaFedex);
        }

        private void GenerarProcesadoresPedidos()
        {
            GenerarClientesFabricas();

            IProcesadorPedidos procesadorPedidosDhl = new ProcesadorPedidos(_clientesFabricas["dhl"]);
            IProcesadorPedidos procesadorPedidosEstafeta = new ProcesadorPedidos(_clientesFabricas["estafeta"]);
            IProcesadorPedidos procesadorPedidosFedex = new ProcesadorPedidos(_clientesFabricas["fedex"]);

            _procesadoresPedidos.Add("dhl", procesadorPedidosDhl);
            _procesadoresPedidos.Add("estafeta", procesadorPedidosEstafeta);
            _procesadoresPedidos.Add("fedex", procesadorPedidosFedex);
            foreach (KeyValuePair<string, IProcesadorPedidos> procesadorPedidos in _procesadoresPedidos)
            {
                AgregarSubscriptores(procesadorPedidos.Value, procesadorPedidos.Key);
            }
        }

        private void AgregarSubscriptores(IProcesadorPedidos procesadorPedidos, string nombreEmpresa)
        {
            foreach (KeyValuePair<string, IClientesFabricas> clienteFabrica in _clientesFabricas)
            {
                if (!clienteFabrica.Key.Equals(nombreEmpresa))
                {
                    procesadorPedidos.SuscribeClienteFabricas((ISuscriberFabricas)clienteFabrica.Value);
                }
            }
        }   
        
        private ParametrosFilasArchivo ObtenerParametrosPedido(string lineaPedido) {
            ParametrosFilasArchivo parametrosFilasArchivo = new ParametrosFilasArchivo();
            string[] parametros = lineaPedido.Split(",");

            if (parametros.Length != 6)
            {
                throw new Exception(string.Format("El numero de columnas para el pedido es incorrecto se esperaban 6 y se tienen {0} para la linea '{1}'.", parametros.Length, lineaPedido));
            }
            else
            {
                parametrosFilasArchivo.Origen = parametros[0].Trim();
                parametrosFilasArchivo.Destino = parametros[1].Trim();
                if (double.TryParse(parametros[2], out double distancia))
                {
                    if (distancia <= 0)
                    {
                        throw new Exception(string.Format("El valor {0} para la distancia es incorrecto.", parametros[2]));
                    }

                    parametrosFilasArchivo.Distancia = distancia;
                }
                else {
                    throw new Exception(string.Format("El valor {0} para la distancia es incorrecto.", parametros[2]));
                }
                parametrosFilasArchivo.Paqueteria = parametros[3].Trim().ToLower();
                parametrosFilasArchivo.MedioTransporte= parametros[4].Trim().ToLower();

                if (DateTime.TryParse(parametros[5], out DateTime fechaPedido))
                {
                    parametrosFilasArchivo.FechaPedido = fechaPedido;
                }
                else
                {
                    throw new Exception(string.Format("La fecha del pedido {0} proporcionado es incorrecto para la linea '{1}'.", parametros[5], lineaPedido));
                }
            }


            return parametrosFilasArchivo;
        }
    }
}
