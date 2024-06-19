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

        // Método para obter todos os produtos
        public async Task<List<Produto>> GetAllProductsAsync()
        {
            return await _dbContext.Produtos.ToListAsync();
        }

        // Método para obter um produto pelo ID
        public async Task<Produto?> GetProductByIdAsync(int id)
        {
            return await _dbContext.Produtos.FindAsync(id);
        }

        // Método para adicionar um novo produto
        public async Task AddProductAsync(Produto produto)
        {
            _dbContext.Produtos.Add(produto);
            await _dbContext.SaveChangesAsync();
        }

        // Método para atualizar um produto existente
        public async Task UpdateProductAsync(Produto produto)
        {
            _dbContext.Entry(produto).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        // Método para deletar um produto pelo ID
        public async Task DeleteProductAsync(int id)
        {
            var produto = await _dbContext.Produtos.FindAsync(id);
            if (produto != null)
            {
                _dbContext.Produtos.Remove(produto);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
