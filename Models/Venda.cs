using System;

namespace Loja.Models
{
    public class Venda
    {
        public int Id { get; set; }
        public DateTime DataVenda { get; set; }
        public string NumNotaFiscal { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int QuantidadeVendida { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}
