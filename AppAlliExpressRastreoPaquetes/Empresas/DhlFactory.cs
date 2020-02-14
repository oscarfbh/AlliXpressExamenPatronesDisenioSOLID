﻿using AppAlliExpressRastreoPaquetes.ClasesAuxiliares;
using AppAlliExpressRastreoPaquetes.Interfaces;
using AppAlliExpressRastreoPaquetes.MediosTransporte;
using System;

namespace AppAlliExpressRastreoPaquetes.Empresas
{
    public class DhlFactory : IEmpresaAbstractFactory
    {
        const double MargenUtilidadPorcentaje = 40;
        const string NombreEmpresa = "DHL";        
        private readonly EstatusCalculos _estatusCalculos;
        private DateTime _fechaPedido;
        private double _distanciaPedido;

        public DhlFactory(DateTime fechaPedido, double distanciaPedido)
        {
            _fechaPedido = fechaPedido;
            _distanciaPedido = distanciaPedido;
            _estatusCalculos = new EstatusCalculos();
        }

        public void AsignarValorFechaPedido(DateTime fecha)
        {
            _fechaPedido = fecha;
        }

        public void AsignarValorDistancia(double distancia)
        {
            _distanciaPedido = distancia;
        }

        public EstatusCalculos CalcularPorAvion()
        {
            Avion avion = new Avion();
            _estatusCalculos.Costo = avion.CalcularCosto(avion.CostoPorKilometro, _distanciaPedido, MargenUtilidadPorcentaje);
            _estatusCalculos.FechaEntrega = avion.CalcularFechaEntrega(_fechaPedido, avion.CalcularTiempoTrasladoHoras(_distanciaPedido));
            return _estatusCalculos;
        }

        public EstatusCalculos CalcularPorBarco()
        {
            Barco barco = new Barco();
            _estatusCalculos.Costo = barco.CalcularCosto(barco.CostoPorKilometro, _distanciaPedido, MargenUtilidadPorcentaje);
            _estatusCalculos.FechaEntrega = barco.CalcularFechaEntrega(_fechaPedido, barco.CalcularTiempoTrasladoHoras(_distanciaPedido));
            return _estatusCalculos;
        }

        public EstatusCalculos CalcularPorTren()
        {
            _estatusCalculos.Mensaje = string.Format("{0} no ofrece el servicio de transporte tren, te recomendamos cotizar en otra empresa.", NombreEmpresa);
            _estatusCalculos.Color = ConsoleColor.Red;
            return _estatusCalculos;
        }

        public string RegresarNombreEmpresa()
        {
            return NombreEmpresa;
        }
    }
}
