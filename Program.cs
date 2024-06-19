using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loja.Data;
using Loja.Models;
using Loja.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Configurar a conexão com o banco de dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LojaDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 26))));

// Registrar ProductService, ClienteService e FornecedorService para injeção de dependência
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<FornecedorService>();

var app = builder.Build();

// Configurar as requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Endpoint para obter todos os produtos
app.MapGet("/produtos", async (ProductService productService) =>
{
    var produtos = await productService.GetAllProductsAsync();
    return Results.Ok(produtos);
});

// Endpoint para obter um produto pelo ID
app.MapGet("/produtos/{id}", async (int id, ProductService productService) =>
{
    var produto = await productService.GetProductByIdAsync(id);
    if (produto == null)
    {
        return Results.NotFound($"Product with ID {id} not found.");
    }
    return Results.Ok(produto);
});

// Endpoint para criar um novo produto
app.MapPost("/createprodutos", async (Produto produto, ProductService productService) =>
{
    await productService.AddProductAsync(produto);
    return Results.Created($"/produtos/{produto.Id}", produto); 
});

// Endpoint para atualizar um produto existente
app.MapPut("/produtos/{id}", async (int id, Produto produto, ProductService productService) =>
{
    if (id != produto.Id)
    {
        return Results.BadRequest("Product ID mismatch.");
    }
    await productService.UpdateProductAsync(produto);
    return Results.Ok();
});

// Endpoint para deletar um produto
app.MapDelete("/produtos/{id}", async (int id, ProductService productService) =>
{
    await productService.DeleteProductAsync(id);
    return Results.Ok();
});

// Endpoint para obter todos os clientes
app.MapGet("/clientes", async (ClienteService clienteService) =>
{
    var clientes = await clienteService.GetAllClientesAsync();
    return Results.Ok(clientes);
});

// Endpoint para obter um cliente pelo ID
app.MapGet("/clientes/{id}", async (int id, ClienteService clienteService) =>
{
    var cliente = await clienteService.GetClienteByIdAsync(id);
    if (cliente == null)
    {
        return Results.NotFound($"Cliente with ID {id} not found.");
    }
    return Results.Ok(cliente);
});

// Endpoint para criar um novo cliente
app.MapPost("/clientes", async (Cliente cliente, ClienteService clienteService) =>
{
    await clienteService.AddClienteAsync(cliente);
    return Results.Created($"/clientes/{cliente.Id}", cliente);
});

// Endpoint para atualizar um cliente existente
app.MapPut("/clientes/{id}", async (int id, Cliente cliente, ClienteService clienteService) =>
{
    if (id != cliente.Id)
    {
        return Results.BadRequest("Cliente ID mismatch.");
    }
    await clienteService.UpdateClienteAsync(cliente);
    return Results.Ok();
});

// Endpoint para deletar um cliente
app.MapDelete("/clientes/{id}", async (int id, ClienteService clienteService) =>
{
    await clienteService.DeleteClienteAsync(id);
    return Results.Ok();
});

// Endpoint para obter todos os fornecedores
app.MapGet("/fornecedores", async (FornecedorService fornecedorService) =>
{
    var fornecedores = await fornecedorService.GetAllFornecedoresAsync();
    return Results.Ok(fornecedores);
});

// Endpoint para obter um fornecedor pelo ID
app.MapGet("/fornecedores/{id}", async (int id, FornecedorService fornecedorService) =>
{
    var fornecedor = await fornecedorService.GetFornecedorByIdAsync(id);
    if (fornecedor == null)
    {
        return Results.NotFound($"Fornecedor with ID {id} not found.");
    }
    return Results.Ok(fornecedor);
});

// Endpoint para criar um novo fornecedor
app.MapPost("/createfornecedores", async (Fornecedor fornecedor, FornecedorService fornecedorService) =>
{
    await fornecedorService.AddFornecedorAsync(fornecedor);
    return Results.Created($"/fornecedores/{fornecedor.Id}", fornecedor);
});

// Endpoint para atualizar um fornecedor existente
app.MapPut("/fornecedores/{id}", async (int id, Fornecedor fornecedor, FornecedorService fornecedorService) =>
{
    if (id != fornecedor.Id)
    {
        return Results.BadRequest("Fornecedor ID mismatch.");
    }
    await fornecedorService.UpdateFornecedorAsync(fornecedor);
    return Results.Ok();
});

// Endpoint para deletar um fornecedor
app.MapDelete("/fornecedores/{id}", async (int id, FornecedorService fornecedorService) =>
{
    await fornecedorService.DeleteFornecedorAsync(id);
    return Results.Ok();
});

app.Run();
