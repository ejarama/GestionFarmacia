using System;
using System.Collections.Generic;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Data.Interfaces
{
    public interface IPedidoRepository
    {
        int CrearPedido(Pedido pedido);
        void InsertarDetallePedido(int pedidoId, DetallePedido detalle);
        List<Pedido> ObtenerPedidosPendientes();
        List<DetallePedido> ObtenerDetallePedido(int pedidoId);
        bool RegistrarRecepcion(int pedidoId, List<DetallePedido> detallesRecibidos);
        void ActualizarCantidadRecibida(int detallePedidoId, int cantidadRecibida);
    }
}
