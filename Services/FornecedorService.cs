using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Loja.Data;
using Loja.Models;

namespace Loja.Services
{
    public class FornecedorService
    {
        private readonly LojaDbContext _dbContext;

        public FornecedorService(LojaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Método para obter todos os fornecedores
        public async Task<List<Fornecedor>> GetAllFornecedoresAsync()
        {
            return await _dbContext.Fornecedores.ToListAsync();
        }

        // Método para obter um fornecedor pelo ID
        public async Task<Fornecedor?> GetFornecedorByIdAsync(int id)
        {
            return await _dbContext.Fornecedores.FindAsync(id);
        }

        // Método para adicionar um novo fornecedor
        public async Task AddFornecedorAsync(Fornecedor fornecedor)
        {
            _dbContext.Fornecedores.Add(fornecedor);
            await _dbContext.SaveChangesAsync();
        }

        // Método para atualizar um fornecedor existente
        public async Task UpdateFornecedorAsync(Fornecedor fornecedor)
        {
            _dbContext.Entry(fornecedor).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        // Método para deletar um fornecedor pelo ID
        public async Task DeleteFornecedorAsync(int id)
        {
            var fornecedor = await _dbContext.Fornecedores.FindAsync(id);
            if (fornecedor != null)
            {
                _dbContext.Fornecedores.Remove(fornecedor);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
