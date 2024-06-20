using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Loja.Data;
using Loja.Models;
using Loja.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("abcabcabcabcabcabcabcabcabcabcabc"))
        };
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LojaDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 26))));

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<FornecedorService>();
builder.Services.AddScoped<VendaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login", async (HttpContext context) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();
    var json = JsonDocument.Parse(body);
    var email = json.RootElement.GetProperty("email").GetString();
    var senha = json.RootElement.GetProperty("senha").GetString();

    // Implementar a validação do usuário e senha aqui.
    // Exemplo simplificado para fins de demonstração:
    if (senha == "1029")
    {
        var token = TokenService.GenerateToken(email);
        await context.Response.WriteAsync(token);
    }
    else
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Credenciais inválidas");
    }
});

app.MapGet("/produtos", async (ProductService productService) =>
{
    var produtos = await productService.GetAllProductsAsync();
    return Results.Ok(produtos);
}).RequireAuthorization();

app.MapGet("/produtos/{id}", async (int id, ProductService productService) =>
{
    var produto = await productService.GetProductByIdAsync(id);
    if (produto == null)
    {
        return Results.NotFound($"Produto com ID {id} não encontrado.");
    }
    return Results.Ok(produto);
}).RequireAuthorization();

app.MapPost("/produtos", async (Produto produto, ProductService productService) =>
{
    await productService.AddProductAsync(produto);
    return Results.Created($"/produtos/{produto.Id}", produto);
}).RequireAuthorization();

app.MapPut("/produtos/{id}", async (int id, Produto produto, ProductService productService) =>
{
    if (id != produto.Id)
    {
        return Results.BadRequest("ID do produto não corresponde.");
    }
    await productService.UpdateProductAsync(produto);
    return Results.Ok();
}).RequireAuthorization();

app.MapDelete("/produtos/{id}", async (int id, ProductService productService) =>
{
    await productService.DeleteProductAsync(id);
    return Results.Ok();
}).RequireAuthorization();

app.MapGet("/clientes", async (ClienteService clienteService) =>
{
    var clientes = await clienteService.GetAllClientesAsync();
    return Results.Ok(clientes);
}).RequireAuthorization();

app.MapGet("/clientes/{id}", async (int id, ClienteService clienteService) =>
{
    var cliente = await clienteService.GetClienteByIdAsync(id);
    if (cliente == null)
    {
        return Results.NotFound($"Cliente com ID {id} não encontrado.");
    }
    return Results.Ok(cliente);
}).RequireAuthorization();

app.MapPost("/clientes", async (Cliente cliente, ClienteService clienteService) =>
{
    await clienteService.AddClienteAsync(cliente);
    return Results.Created($"/clientes/{cliente.Id}", cliente);
}).RequireAuthorization();

app.MapPut("/clientes/{id}", async (int id, Cliente cliente, ClienteService clienteService) =>
{
    if (id != cliente.Id)
    {
        return Results.BadRequest("ID do cliente não corresponde.");
    }
    await clienteService.UpdateClienteAsync(cliente);
    return Results.Ok();
}).RequireAuthorization();

app.MapDelete("/clientes/{id}", async (int id, ClienteService clienteService) =>
{
    await clienteService.DeleteClienteAsync(id);
    return Results.Ok();
}).RequireAuthorization();

app.MapGet("/fornecedores", async (FornecedorService fornecedorService) =>
{
    var fornecedores = await fornecedorService.GetAllFornecedoresAsync();
    return Results.Ok(fornecedores);
}).RequireAuthorization();

app.MapGet("/fornecedores/{id}", async (int id, FornecedorService fornecedorService) =>
{
    var fornecedor = await fornecedorService.GetFornecedorByIdAsync(id);
    if (fornecedor == null)
    {
        return Results.NotFound($"Fornecedor com ID {id} não encontrado.");
    }
    return Results.Ok(fornecedor);
}).RequireAuthorization();

app.MapPost("/fornecedores", async (Fornecedor fornecedor, FornecedorService fornecedorService) =>
{
    await fornecedorService.AddFornecedorAsync(fornecedor);
    return Results.Created($"/fornecedores/{fornecedor.Id}", fornecedor);
}).RequireAuthorization();

app.MapPut("/fornecedores/{id}", async (int id, Fornecedor fornecedor, FornecedorService fornecedorService) =>
{
    if (id != fornecedor.Id)
    {
        return Results.BadRequest("ID do fornecedor não corresponde.");
    }
    await fornecedorService.UpdateFornecedorAsync(fornecedor);
    return Results.Ok();
}).RequireAuthorization();

app.MapDelete("/fornecedores/{id}", async (int id, FornecedorService fornecedorService) =>
{
    await fornecedorService.DeleteFornecedorAsync(id);
    return Results.Ok();
}).RequireAuthorization();

// Rotas para Vendas
app.MapPost("/vendas", async (Venda venda, VendaService vendaService) =>
{
    var result = await vendaService.AddVendaAsync(venda);
    if (result)
    {
        return Results.Created($"/vendas/{venda.Id}", venda);
    }
    return Results.BadRequest("Cliente ou produto não encontrado.");
}).RequireAuthorization();

app.MapGet("/vendas/produto/{produtoId}", async (int produtoId, VendaService vendaService) =>
{
    var vendas = await vendaService.GetVendasByProdutoDetalhadaAsync(produtoId);
    return Results.Ok(vendas.Select(v => new
    {
        NomeProduto = v.Produto.Nome,
        DataVenda = v.DataVenda,
        IdVenda = v.Id,
        NomeCliente = v.Cliente.Nome,
        QuantidadeVendida = v.QuantidadeVendida,
        PrecoVenda = v.PrecoUnitario
    }));
}).RequireAuthorization();

app.MapGet("/vendas/produto/{produtoId}/sumarizada", async (int produtoId, VendaService vendaService) =>
{
    var venda = await vendaService.GetVendasByProdutoSumarizadaAsync(produtoId);
    return Results.Ok(venda);
}).RequireAuthorization();

app.MapGet("/vendas/cliente/{clienteId}", async (int clienteId, VendaService vendaService) =>
{
    var vendas = await vendaService.GetVendasByClienteDetalhadaAsync(clienteId);
    return Results.Ok(vendas.Select(v => new
    {
        NomeProduto = v.Produto.Nome,
        DataVenda = v.DataVenda,
        IdVenda = v.Id,
        QuantidadeVendida = v.QuantidadeVendida,
        PrecoVenda = v.PrecoUnitario
    }));
}).RequireAuthorization();

app.MapGet("/vendas/cliente/{clienteId}/sumarizada", async (int clienteId, VendaService vendaService) =>
{
    var venda = await vendaService.GetVendasByClienteSumarizadaAsync(clienteId);
    return Results.Ok(venda);
}).RequireAuthorization();

app.Run();
