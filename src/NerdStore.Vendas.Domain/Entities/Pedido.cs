using FluentValidation.Results;
using NerdStore.SharedKernel.DomainObjects;
using NerdStore.SharedKernel.Exceptions;
using NerdStore.Vendas.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NerdStore.Vendas.Domain.Entities
{
    public class Pedido : Entity, IAggregateRoot
    {
        public int Codigo { get; private set; }
        public Guid ClienteId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool VoucherAplicado { get; private set; }
        public decimal Desconto { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public PedidoStatus Status { get; private set; }

        private readonly List<PedidoItem> _pedidoItems;
        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;
        public virtual Voucher Voucher { get; private set; }

        public Pedido(Guid clienteId, bool voucherAplicado, decimal desconto, decimal valorTotal)
        {
            this.ClienteId = clienteId;
            this.VoucherAplicado = voucherAplicado;
            this.Desconto = desconto;
            this.ValorTotal = valorTotal;
            this._pedidoItems = new List<PedidoItem>();
        }

        protected Pedido() => this._pedidoItems = new List<PedidoItem>();

        public bool ItemJaExistente(PedidoItem item) => _pedidoItems.Any(pi => pi.ProdutoId == item.ProdutoId);

        public ValidationResult AplicarVoucher(Voucher voucher)
        {
            var voucherResult = voucher.ehAplicavel();
            if (!voucherResult.IsValid) return voucherResult;

            Voucher = voucher;
            VoucherAplicado = true;
            Total();

            return voucherResult;
        }

        public void Total()
        {
            this.ValorTotal = PedidoItems.Sum(x => x.Calcular());
            CalculaDesconto();
        }

        public void AdicionarItem(PedidoItem item)
        {
            if (!item.IsValid()) return;

            item.AssociarPedido(Id);
            if (ItemJaExistente(item))
            {
                var _itemExistente = _pedidoItems.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);
                _itemExistente.AdicionarUnidades(item.Quantidade);
                item = _itemExistente;

                _pedidoItems.Remove(_itemExistente);
            }

            item.Calcular();
            _pedidoItems.Add(item);
            Total();
        }

        public void AtualizarItem(PedidoItem item)
        {
            if (!item.IsValid()) return;

            item.AssociarPedido(Id);

            var _itemExistente = _pedidoItems.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);
            if (_itemExistente == null)
                throw new DomainException("Item não encontrado");

            _pedidoItems.Remove(_itemExistente);
            _pedidoItems.Add(item);

            Total();
        }

        public void RemoverItem(PedidoItem item)
        {
            if (!item.IsValid()) return;

            var _itemExistente = _pedidoItems.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);
            if (_itemExistente == null)
                throw new DomainException("Item não encontrado");

            _pedidoItems.Remove(_itemExistente);

            Total();
        }

        public void AtualizarUnidades(PedidoItem item, int unidades)
        {
            item.AtualizarUnidades(unidades);
            AtualizarItem(item);
        }

        #region Status

        public void TornarRascunho()
        {
            Status = PedidoStatus.Rascunho;
        }

        public void Iniciar()
        {
            Status = PedidoStatus.Iniciado;
        }

        public void Cancelar()
        {
            Status = PedidoStatus.Cancelado;
        }

        public void Pago()
        {
            Status = PedidoStatus.Pago;
        }

        #endregion Status

        public void CalculaDesconto()
        {
            if (!VoucherAplicado) return;

            VoucherPorcentagem();
            VoucherValor();
        }

        private void VoucherValor()
        {
            if (Voucher.TipoDescontoVoucher != TipoDescontoVoucher.Valor
               && !Voucher.ValorDesconto.HasValue)
                return;

            var _valor = ValorTotal;

            decimal _desconto = Voucher.ValorDesconto.Value;
            _valor -= _desconto;

            ValorTotal = _valor < 0 ? 0 : _valor;
            Desconto = _desconto;
        }

        private void VoucherPorcentagem()
        {
            if (Voucher.TipoDescontoVoucher != TipoDescontoVoucher.Porcentagem
                && !Voucher.Percentual.HasValue)
                return;

            var _valor = ValorTotal;

            decimal _desconto = (_valor + Voucher.Percentual.Value) / 100;
            _valor -= _desconto;

            ValorTotal = _valor < 0 ? 0 : _valor;
            Desconto = _desconto;
        }

        public static class PedidoFactory
        {
            public static Pedido NovoPedidoRascunho(Guid clienteId)
            {
                var pedido = new Pedido
                {
                    ClienteId = clienteId,
                };

                pedido.TornarRascunho();
                return pedido;
            }
        }
    }
}