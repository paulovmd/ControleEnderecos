using ControleEnderecos.Application;
using ControleEnderecos.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddRouting();
//Gera a documenta��o da API
builder.Services.AddSwaggerGen();
//Adicionando os Profiles do AutoMapper
builder.Services.AddAutoMapper(typeof(ApplicationProfile));

//Adiciona o mecanismo de inje��o de dep�ncia da classe,
//respons�vel pela conex�o com o banco de dados.
string? connectionString = builder.Configuration
    .GetConnectionString("ControleEnderecoContext");

builder.Services.AddDbContext<ControleEnderecoContext>(
    optionsAction: options => 
    options.UseMySQL(connectionString!));

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
