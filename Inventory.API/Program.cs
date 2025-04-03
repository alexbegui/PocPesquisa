using CensusFieldSurvey.API;
using CensusFieldSurvey.DataBase;
using CensusFieldSurvey.Model.EntitesBD;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiEndpointsOptions>(builder.Configuration.GetSection("ApiEndpoints"));

var configuration = builder.Configuration;

// Configura o cliente HTTP para comunicação com o serviço SMTI.
builder.Services.AddHttpClient();

// Configura os controladores da API para usar System.Text.Json e ignora referências circulares durante a serialização JSON.
// Isso evita erros quando há relacionamentos recursivos entre objetos.
builder.Services.AddControllers().AddJsonOptions(op => op.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Adiciona o suporte para a descoberta de endpoints da API.
builder.Services.AddEndpointsApiExplorer();

// Adicionando Swagger para documentar e testar a API
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "API Bora Pesquisar", Version = "v1" });
    // Configurações de segurança para JWT
    // c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    // {
    //     Description = "Informe o token desta forma: Bearer {seu token}",
    //     Name = "Authorization",
    //     In = ParameterLocation.Header,
    //     Type = SecuritySchemeType.ApiKey,
    //     Scheme = "Bearer"
    // });

    // c.AddSecurityRequirement(new OpenApiSecurityRequirement
    // {
    //     {
    //         new OpenApiSecurityScheme
    //         {
    //             Reference = new OpenApiReference
    //             {
    //                 Type = ReferenceType.SecurityScheme,
    //                 Id = "Bearer"
    //             },
    //             Scheme = "Bearer",
    //             Name = "Bearer",
    //             In = ParameterLocation.Header,
    //         },
    //         new List<string>()
    //     }
    // });
});

var ambienteConnectionString = configuration.GetValue<string>("ConnectionStrings:AmbienteConnectionStrings");
var connectionString = ambienteConnectionString switch
{
    "Local" => configuration.GetConnectionString("Banco_local"),
    "Homologacao" => configuration.GetConnectionString("Banco_homologacao"),
    "Producao" => configuration.GetConnectionString("Banco_producao"),
    _ => throw new InvalidOperationException("Ambiente de conexão desconhecido.")
};

if (connectionString == null)
    throw new InvalidOperationException("Ambiente de conexão desconhecido.");

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(connectionString)
);

// Repositórios para acesso aos dados.
// Registra AssetRepository e ParameterRepository como implementações de IRepository para InventoryEntry e Parameter, respectivamente.
// O escopo 'Scoped' garante que uma nova instância do repositório seja criada para cada requisição HTTP.
builder.Services.AddScoped<IRepository<Research>, ResearchRepository>();

// Configuração do CORS (Cross-Origin Resource Sharing) para permitir requisições de diferentes origens.
if (builder.Environment.IsDevelopment())
{
    // Configuração para ambiente de desenvolvimento: permite qualquer origem, método e cabeçalho.
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });
}
else
{
    // Configuração para ambiente de produção:  permite apenas origens especificadas na configuração 'AllowedOrigins'.
    // Garante mais segurança, restringindo o acesso à API apenas para domínios autorizados.
    var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

    if (allowedOrigins == null || allowedOrigins.Length == 0)
    {
        throw new InvalidOperationException("A configuração 'AllowedOrigins' está ausente ou vazia no arquivo de configuração.");
    }

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", policy =>
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
    });
}

// Configura o AutoMapper para mapear objetos entre diferentes tipos.
builder.Services.AddAutoMapper(typeof(Program));

// Adicionando autorização global
// builder.Services.AddAuthorization();

var app = builder.Build();

// Esse aqui é para interceptar as exceções que tem para depois jogar no Swagger, para facilitar o problema.
//app.UseMiddleware<ErrorHandlingMiddleware>();

// Aplica a política CORS configurada.
//app.UseCors("CorsPolicy");

// Configura o pipeline de middleware HTTP.
// if (app.Environment.IsDevelopment())
// {

// }

// Em ambiente de desenvolvimento, usa as páginas de erro do desenvolvedor e o Swagger para auxiliar no desenvolvimento e teste da API.
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection(); // Redireciona requisições HTTP para HTTPS.
// app.UseAuthentication(); // Adiciona a autenticação ao pipeline.
// app.UseAuthorization(); // Adiciona a autorização ao pipeline.
app.MapControllers(); // Mapeia os controllers para os endpoints da API.
app.Run(); // Inicia a aplicação.