using Loja.Data;
using Loja.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loja.Services
{
    public class VendaService
    {
        private readonly LojaDbContext _context;

        public VendaService(LojaDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddVendaAsync(Venda venda)
        {
            // Validação se o cliente e produto existem
            var cliente = await _context.Clientes.FindAsync(venda.ClienteId);
            var produto = await _context.Produtos.FindAsync(venda.ProdutoId);

            if (cliente == null || produto == null)
            {
                return false;
            }

            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Venda>> GetVendasByProdutoDetalhadaAsync(int produtoId)
        {
            return await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Produto)
                .Where(v => v.ProdutoId == produtoId)
                .ToListAsync();
        }

        public async Task<dynamic> GetVendasByProdutoSumarizadaAsync(int produtoId)
        {
            return await _context.Vendas
                .Where(v => v.ProdutoId == produtoId)
                .GroupBy(v => v.ProdutoId)
                .Select(g => new
                {
                    NomeProduto = g.First().Produto.Nome,
                    QuantidadeTotalVendida = g.Sum(v => v.QuantidadeVendida),
                    PrecoTotalVendido = g.Sum(v => v.PrecoUnitario * v.QuantidadeVendida)
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<Venda>> GetVendasByClienteDetalhadaAsync(int clienteId)
        {
            return await _context.Vendas
                .Include(v => v.Produto)
                .Where(v => v.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task<dynamic> GetVendasByClienteSumarizadaAsync(int clienteId)
        {
            return await _context.Vendas
                .Where(v => v.ClienteId == clienteId)
                .GroupBy(v => v.ClienteId)
                .Select(g => new
                {
                    QuantidadeTotalVendida = g.Sum(v => v.QuantidadeVendida),
                    PrecoTotalVendido = g.Sum(v => v.PrecoUnitario * v.QuantidadeVendida)
                })
                .FirstOrDefaultAsync();
        }
    }
}
