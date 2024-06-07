using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Loja.Data;
using Loja.Models;
namespace Loja.Services
{
    public class ProductService
    {
        private readonly LojaDbContext _dbContext;
        public ProductService(LojaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // Método para consultar todos os produtos
        public async Task<List<Produto>> GetAllProductsAsync()
        {
            return await _dbContext.Produtos.ToListAsync();
        }
        // Métodd para consultar um produto a partir do seu Id
        public async Task<Produto> GetProductByIdAsync(int Id)
        {
            return await _dbContext.Produtos.FindAsync(Id);
        }
        // Método para gravar um novo produto
        public async Task AddProductAsync(Produto produto)
        {
            _dbContext.Produtos.Add(produto);
            await _dbContext.SaveChangesAsync();
        }
        // Método para atualizar os dados de um produto
        public async Task UpdateProductAsync(Produto produto)
        {
            _dbContext.Entry(produto).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        // Método para excluir um produto
        public async Task DeleteProductAsync(int Id)
        {
            var produto = await _dbContext.Produtos.FindAsync(Id);
            if (produto != null)
            {
                _dbContext.Produtos.Remove(produto);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}