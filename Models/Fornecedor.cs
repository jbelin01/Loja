using System.ComponentModel.DataAnnotations;

namespace Loja.Models
{
    public class Fornecedor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Cnpj { get; set; } = string.Empty; 

        [Required]
        public string Nome { get; set; } = string.Empty; 

        [Required]
        public string Endereco { get; set; } = string.Empty; 

        [Required]
        public string Email { get; set; } = string.Empty; 

        [Required]
        public string Telefone { get; set; } = string.Empty; 
    }
}
