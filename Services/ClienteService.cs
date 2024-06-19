using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Loja.Data;
using Loja.Models;

namespace Loja.Services
{
    public class ClienteService
    {
        private readonly LojaDbContext _dbContext;

        public ClienteService(LojaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Método para obter todos os clientes
        public async Task<List<Cliente>> GetAllClientesAsync()
        {
            return await _dbContext.Clientes.ToListAsync();
        }

        // Método para obter um cliente pelo ID
        public async Task<Cliente?> GetClienteByIdAsync(int id)
        {
            return await _dbContext.Clientes.FindAsync(id);
        }

        // Método para adicionar um novo cliente
        public async Task AddClienteAsync(Cliente cliente)
        {
            _dbContext.Clientes.Add(cliente);
            await _dbContext.SaveChangesAsync();
        }

        // Método para atualizar um cliente existente
        public async Task UpdateClienteAsync(Cliente cliente)
        {
            _dbContext.Entry(cliente).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        // Método para deletar um cliente pelo ID
        public async Task DeleteClienteAsync(int id)
        {
            var cliente = await _dbContext.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _dbContext.Clientes.Remove(cliente);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
