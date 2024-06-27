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

// Configuração da autenticação usando JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Desabilitar validação de emissor e audiência
            ValidateIssuer = false,
            ValidateAudience = false,
            // Habilitar validação da chave de assinatura
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("abcabcabcabcabcabcabcabcabcabcabc"))
        };
    });

// Configuração da conexão com o banco de dados MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LojaDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 26))));

// Registro dos serviços no contêiner de injeção de dependências
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<FornecedorService>();
builder.Services.AddScoped<VendaService>();
builder.Services.AddAuthorization(); // Adicionar serviços de autorização

var app = builder.Build();

// Configuração para o ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection(); // Redirecionamento de HTTP para HTTPS
app.UseAuthentication();   // Habilitar autenticação
app.UseAuthorization();    // Habilitar autorização

// Rota para login
app.MapPost("/login", async (HttpContext context) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();
    var json = JsonDocument.Parse(body);
    var email = json.RootElement.GetProperty("email").GetString();
    var senha = json.RootElement.GetProperty("senha").GetString();

    // Implementar a validação do usuário e senha aqui.
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

// ----------------------------PRODUTOS---------------------------------------------------------------------------

// Rota para obter todos os produtos
app.MapGet("/produtos", async (ProductService productService) =>
{
    var produtos = await productService.GetAllProductsAsync();
    return Results.Ok(produtos);
});

// Rota para obter um produto por ID
app.MapGet("/produtos/{id}", async (int id, ProductService productService) =>
{
    var produto = await productService.GetProductByIdAsync(id);
    if (produto == null)
    {
        return Results.NotFound($"Produto com ID {id} não encontrado.");
    }
    return Results.Ok(produto);
});

// Rota para adicionar um novo produto
app.MapPost("/produtos", async (Produto produto, ProductService productService) =>
{
    await productService.AddProductAsync(produto);
    return Results.Created($"/produtos/{produto.Id}", produto);
});

// Rota para atualizar um produto existente
app.MapPut("/produtos/{id}", async (int id, Produto produto, ProductService productService) =>
{
    if (id != produto.Id)
    {
        return Results.BadRequest("ID do produto não corresponde.");
    }
    await productService.UpdateProductAsync(produto);
    return Results.Ok();
});

// Rota para deletar um produto por ID
app.MapDelete("/produtos/{id}", async (int id, ProductService productService) =>
{
    await productService.DeleteProductAsync(id);
    return Results.Ok();
});

// ----------------------------CLIENTES---------------------------------------------------------------------------

// Rota para obter todos os clientes
app.MapGet("/clientes", async (ClienteService clienteService) =>
{
    var clientes = await clienteService.GetAllClientesAsync();
    return Results.Ok(clientes);
});

// Rota para obter um cliente por ID
app.MapGet("/clientes/{id}", async (int id, ClienteService clienteService) =>
{
    var cliente = await clienteService.GetClienteByIdAsync(id);
    if (cliente == null)
    {
        return Results.NotFound($"Cliente com ID {id} não encontrado.");
    }
    return Results.Ok(cliente);
});

// Rota para adicionar um novo cliente
app.MapPost("/clientes", async (Cliente cliente, ClienteService clienteService) =>
{
    await clienteService.AddClienteAsync(cliente);
    return Results.Created($"/clientes/{cliente.Id}", cliente);
});

// Rota para atualizar um cliente existente
app.MapPut("/clientes/{id}", async (int id, Cliente cliente, ClienteService clienteService) =>
{
    if (id != cliente.Id)
    {
        return Results.BadRequest("ID do cliente não corresponde.");
    }
    await clienteService.UpdateClienteAsync(cliente);
    return Results.Ok();
});

// Rota para deletar um cliente por ID
app.MapDelete("/clientes/{id}", async (int id, ClienteService clienteService) =>
{
    await clienteService.DeleteClienteAsync(id);
    return Results.Ok();
});

// ----------------------------FORNECEDORES---------------------------------------------------------------------------

// Rota para obter todos os fornecedores
app.MapGet("/fornecedores", async (FornecedorService fornecedorService) =>
{
    var fornecedores = await fornecedorService.GetAllFornecedoresAsync();
    return Results.Ok(fornecedores);
});

// Rota para obter um fornecedor por ID
app.MapGet("/fornecedores/{id}", async (int id, FornecedorService fornecedorService) =>
{
    var fornecedor = await fornecedorService.GetFornecedorByIdAsync(id);
    if (fornecedor == null)
    {
        return Results.NotFound($"Fornecedor com ID {id} não encontrado.");
    }
    return Results.Ok(fornecedor);
});

// Rota para adicionar um novo fornecedor
app.MapPost("/fornecedores", async (Fornecedor fornecedor, FornecedorService fornecedorService) =>
{
    await fornecedorService.AddFornecedorAsync(fornecedor);
    return Results.Created($"/fornecedores/{fornecedor.Id}", fornecedor);
});

// Rota para atualizar um fornecedor existente
app.MapPut("/fornecedores/{id}", async (int id, Fornecedor fornecedor, FornecedorService fornecedorService) =>
{
    if (id != fornecedor.Id)
    {
        return Results.BadRequest("ID do fornecedor não corresponde.");
    }
    await fornecedorService.UpdateFornecedorAsync(fornecedor);
    return Results.Ok();
});

// Rota para deletar um fornecedor por ID
app.MapDelete("/fornecedores/{id}", async (int id, FornecedorService fornecedorService) =>
{
    await fornecedorService.DeleteFornecedorAsync(id);
    return Results.Ok();
});

// ----------------------------VENDAS---------------------------------------------------------------------------

// Rota para adicionar uma nova venda
app.MapPost("/vendas", async (Venda venda, VendaService vendaService) =>
{
    var result = await vendaService.AddVendaAsync(venda);
    if (result)
    {
        return Results.Created($"/vendas/{venda.Id}", venda);
    }
    return Results.BadRequest("Cliente ou produto não encontrado.");
});

// Rota para obter vendas detalhadas por produto
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
});

// Rota para obter vendas sumarizadas por produto
app.MapGet("/vendas/produto/{produtoId}/sumarizada", async (int produtoId, VendaService vendaService) =>
{
    var venda = await vendaService.GetVendasByProdutoSumarizadaAsync(produtoId);
    return Results.Ok(venda);
});

// Rota para obter vendas detalhadas por cliente
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
});

// Rota para obter vendas sumarizadas por cliente
app.MapGet("/vendas/cliente/{clienteId}/sumarizada", async (int clienteId, VendaService vendaService) =>
{
    var venda = await vendaService.GetVendasByClienteSumarizadaAsync(clienteId);
    return Results.Ok(venda);
});

app.Run(); // Iniciar o aplicativo
