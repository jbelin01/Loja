using Microsoft.EntityFrameworkCore;
using Loja.Data;
using Loja.Models;


var builder = WebApplication.CreateBuilder(args);

// Configurar a conexão com o BD
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LojaDbContext>(options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 26))));

var app = builder.Build();

app.UseHttpsRedirection();

// Produto -------------------------------------------------------------------------------------------------------------------------------------------------------------------------

app.MapPost("/createproduto", async (LojaDbContext dbContext, Produto newProduto) => {

    dbContext.Produtos.Add(newProduto);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/createproduto/{newProduto.Id}", newProduto);

});

app.MapGet("/produtos", async (LojaDbContext dbContext) => 
{

    var produtos = await dbContext.Produtos.ToListAsync();
    return Results.Ok(produtos);

});

app.MapGet("/produtos/{Id}", async (int Id, LojaDbContext dbContext) => 
{

    var produtos = await dbContext.Produtos.FindAsync(Id);
    if (produtos == null)
    {

        return Results.NotFound($"Produto with ID {Id} not found. ");

    }

    return Results.Ok(produtos);
});

// Endpoint para atualizar um Produto existente
app.MapPut("/produtos/{Id}", async (int Id, LojaDbContext dbContext, Produto updatedProduto) =>
{
    //Verifica se o produto existe na base, conforme o id informado
    //Se o produto existir na base, será retornado para dentro do objeto existingProduto
    var existingProduto = await dbContext.Produtos.FindAsync(Id);
    if (existingProduto == null)
    {
        return Results.NotFound($"Produto with ID {Id} not found.");
    }
    //Atualiza os dados do existingProduto
    existingProduto.Nome = updatedProduto.Nome;
    existingProduto.Preco = updatedProduto.Preco;
    existingProduto.Fornecedor = updatedProduto.Fornecedor;
    //Salva no banco de dados
    await dbContext.SaveChangesAsync();
    //Retorna para o cliente que invocou o endpoint
    return Results.Ok(existingProduto);
});


// Cliente -------------------------------------------------------------------------------------------------------------------------------------------------------------------------

app.MapPost("/createcliente", async (LojaDbContext dbContext, Cliente newCliente) =>
{
    dbContext.Clientes.Add(newCliente);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/createcliente/{newCliente.Id}", newCliente);
});

app.MapGet("/clientes", async (LojaDbContext dbContext) => 
{

    var clientes = await dbContext.Clientes.ToListAsync();
    return Results.Ok(clientes);

});

app.MapGet("/clientes/{Id}", async (int Id, LojaDbContext dbContext) => 
{

    var clientes = await dbContext.Clientes.FindAsync(Id);
    if (clientes == null)
    {

        return Results.NotFound($"Cliente with ID {Id} not found. ");

    }

    return Results.Ok(clientes);
});

// Endpoint para atualizar um Cliente existente
app.MapPut("/clientes/{Id}", async (int Id, LojaDbContext dbContext, Cliente updatedCliente) =>
{
    //Verifica se o Cliente existe na base, conforme o id informado
    //Se o Cliente existir na base, será retornado para dentro do objeto existingCliente
    var existingCliente = await dbContext.Clientes.FindAsync(Id);
    if (existingCliente == null)
    {
        return Results.NotFound($"Cliente with ID {Id} not found.");
    }
    //Atualiza os dados do existingCliente
    existingCliente.Nome = updatedCliente.Nome;
    existingCliente.Cpf = updatedCliente.Cpf;
    existingCliente.Email = updatedCliente.Email;
    //Salva no banco de dados
    await dbContext.SaveChangesAsync();
    //Retorna para o cliente que invocou o endpoint
    return Results.Ok(existingCliente);
});


// Fornecedor -------------------------------------------------------------------------------------------------------------------------------------------------------------------

app.MapPost("/createfornecedores", async (LojaDbContext dbContext, Fornecedor newFornecedor) =>
{
    dbContext.Fornecedores.Add(newFornecedor);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/createfornecedores/{newFornecedor.Id}", newFornecedor);
});

app.MapGet("/fornecedores", async (LojaDbContext dbContext) => 
{

    var fornecedores = await dbContext.Fornecedores.ToListAsync();
    return Results.Ok(fornecedores);

});

app.MapGet("/fornecedores/{Id}", async (int Id, LojaDbContext dbContext) => 
{

    var fornecedores = await dbContext.Fornecedores.FindAsync(Id);
    if (fornecedores == null)
    {

        return Results.NotFound($"Fornecedor with ID {Id} not found. ");

    }

    return Results.Ok(fornecedores);
});

// Endpoint para atualizar um Fornecedor existente
app.MapPut("/fornecedores/{Id}", async (int Id, LojaDbContext dbContext, Fornecedor updatedFornecedor) =>
{
    //Verifica se o Fornecedor existe na base, conforme o id informado
    //Se o Fornecedor existir na base, será retornado para dentro do objeto existingFornecedor
    var existingFornecedor = await dbContext.Fornecedores.FindAsync(Id);
    if (existingFornecedor == null)
    {
        return Results.NotFound($"Fornecedor with ID {Id} not found.");
    }
    //Atualiza os dados do existingFornecedor
    existingFornecedor.Cnpj = updatedFornecedor.Cnpj;
    existingFornecedor.Nome = updatedFornecedor.Nome;
    existingFornecedor.Endereco = updatedFornecedor.Endereco;
    existingFornecedor.Email = updatedFornecedor.Email;
    existingFornecedor.Telefone = updatedFornecedor.Telefone;
    //Salva no banco de dados
    await dbContext.SaveChangesAsync();
    //Retorna para o Fornecedor que invocou o endpoint
    return Results.Ok(existingFornecedor);
});

app.Run();
