using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loja.Models
{
    public class Venda
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DataVenda { get; set; }

        [Required]
        public string? NumNotaFiscal { get; set; } 

        [Required]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; } 

        [Required]
        public int ProdutoId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto? Produto { get; set; } 

        [Required]
        public int QuantidadeVendida { get; set; }

        [Required]
        public decimal PrecoUnitario { get; set; }
    }
}
