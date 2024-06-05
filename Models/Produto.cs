using System.ComponentModel.DataAnnotations;

namespace Loja.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Preco { get; set; }
        public string Fornecedor { get; set; }
    }
}