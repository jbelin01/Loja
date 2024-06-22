using System.ComponentModel.DataAnnotations;

namespace Loja.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty; 

        [Required]
        public string Cpf { get; set; } = string.Empty; 

        [Required]
        public string Email { get; set; } = string.Empty; 
    }
}
