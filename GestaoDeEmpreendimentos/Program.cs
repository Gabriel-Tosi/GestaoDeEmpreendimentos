using Microsoft.EntityFrameworkCore;
using GestaoDeEmpreendimentos.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao container.

// Adiciona DbContext para MySQL
var conn = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<GestaoDeEmpreendimentos.Data.AppDbContext>(options =>
    options.UseMySql(conn, Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(conn)));

// Adiciona AutoMapper
builder.Services.AddAutoMapper(typeof(GestaoDeEmpreendimentos.Profiles.EmpreendimentoProfile));

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var problem = new
            {
                Mensagem = "Falha de validação",
                Erros = errors
            };

            return new BadRequestObjectResult(problem);
        };
    });
// Configurações do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API de Gestão de Empreendimentos",
        Version = "v1",
        Description = "API para gerenciamento de empreendimentos."
    });

    // Inclui comentários XML gerados pelo build (localiza o arquivo na pasta de saída do app)
    var xmlFile = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".xml";
    // Tenta localizar o XML na pasta de saída do app
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (!System.IO.File.Exists(xmlPath))
    {
        // Fallback: tenta localizar no diretório do projeto (útil em alguns cenários de build)
        var fallback = System.IO.Path.Combine(Directory.GetCurrentDirectory(), xmlFile);
        if (System.IO.File.Exists(fallback)) xmlPath = fallback;
    }

    if (System.IO.File.Exists(xmlPath))
    {
        // Inclui comentários XML e instrui o Swagger a também ler comentários de controllers/ações
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
