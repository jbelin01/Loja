using System.ComponentModel.DataAnnotations;

namespace Loja.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty; 

        [Required]
        public decimal Preco { get; set; }

        [Required]
        public string Fornecedor { get; set; } = string.Empty; 
    }
}
