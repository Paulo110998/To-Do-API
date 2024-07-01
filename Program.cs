using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using TO_DO___API.Data;
using Microsoft.EntityFrameworkCore;
using TO_DO___API.Models;
using TO_DO___API.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TO_DO___API.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole(); 
builder.Services.AddMemoryCache();

// Buscando conexão com database para as tabelas das entidades
var entidadesConnectionString = builder.Configuration.GetConnectionString("EntidadesConnection");

builder.Services.AddDbContext<EntidadesContext>(opt =>
    opt.UseLazyLoadingProxies().UseMySql(entidadesConnectionString, ServerVersion.AutoDetect(entidadesConnectionString)));

// Buscando conexão com database para as tabelas de usuários
var usersConnectionString = builder.Configuration.GetConnectionString("UsuariosConnection");


// Adicionando o services
builder.Services.AddDbContext<UsuariosContext>(opt =>
{
    opt.UseMySql(usersConnectionString, ServerVersion.AutoDetect(usersConnectionString));
});

builder.Services
    .AddIdentity<Usuario, IdentityRole>()
    .AddEntityFrameworkStores<UsuariosContext>()
    .AddDefaultTokenProviders();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<UsuariosService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<ListasService>();
builder.Services.AddScoped<TarefasService>();
builder.Services.AddScoped<AnotacoesService>();
builder.Services.AddScoped<FamiliaService>();
builder.Services.AddScoped<MembroFamiliaService>();
builder.Services.AddScoped<GeradorSenhasService>();

// Configuração de Password de usuário
builder.Services.Configure<IdentityOptions>(options =>
{
    // Define o comprimento mínimo necessário para a senha
    options.Password.RequiredLength = 10;

    // Exige que a senha contenha pelo menos um dígito numérico (0-9)
    options.Password.RequireDigit = true;

    // Exige que a senha contenha pelo menos uma letra minúscula (a-z)
    options.Password.RequireLowercase = true;

    // Define que não é necessário que a senha contenha caracteres não alfanuméricos (como @, #, $)
    options.Password.RequireNonAlphanumeric = false;

    // Exige que a senha contenha pelo menos uma letra maiúscula (A-Z)
    options.Password.RequireUppercase = true;

    // Especifica o número mínimo de caracteres únicos que a senha deve conter
    options.Password.RequiredUniqueChars = 1;
});

// Gera uma chave forte para assinar o token JWT
// (Sempre que o servidor for iniciado, gera uma nova chave)
var securityKey = new byte[32]; // 32 bytes = 256 bits
using (var generator = RandomNumberGenerator.Create())
{
    generator.GetBytes(securityKey);
}

// Configurar a chave de segurança para o JWT
builder.Services.AddSingleton(new SymmetricSecurityKey(securityKey));

// Adicionando serviço de autenticação por JWT 
builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opts =>
{
    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(securityKey),
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero
    };
});

//Adicionando cookie de redirecionamento..
builder.Services.ConfigureApplicationCookie(options => {
    options.Events.OnRedirectToAccessDenied = context => {
        context.Response.StatusCode = 403;
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});



builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen();

// Adicionando autorizações de acesso aos dados
builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("AuthenticationUser", policy =>
    {
        policy.AddRequirements(new AuthenticationUser());
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, EntidadesAuthorization>();

// Adicionando o mapeamento de DTOs (AutoMapper)
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configurando proteção de dados
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/app/ExternalDataProtectionKeys"))
    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
    {
        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
    });

// Adiciona a política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Adiciona o HttpClient para o GeradorSenhasService
builder.Services.AddHttpClient<GeradorSenhasService>(client =>
{
    // Define a URL base para o HttpClient. Esta é a parte comum da URL para todas as solicitações feitas por este cliente.
    client.BaseAddress = new Uri("https://password-generator-by-api-ninjas.p.rapidapi.com/v1/");

    // Adiciona o cabeçalho "x-rapidapi-key" com a chave de API necessária para autenticar as solicitações.
    // Esta chave deve ser mantida em segredo e não deve ser exposta publicamente.
    client.DefaultRequestHeaders.Add("x-rapidapi-key", "f3d3fd1e1bmsh0bbecb399458a08p1c3e13jsn99e07e0be825");

    // Adiciona o cabeçalho "x-rapidapi-host" com o host da API. Este cabeçalho é utilizado pela API para roteamento e verificação.
    client.DefaultRequestHeaders.Add("x-rapidapi-host", "password-generator-by-api-ninjas.p.rapidapi.com");
});


var app = builder.Build();

// Usa a política de CORS
app.UseCors("CorsPolicy");

// Ambiente de produção
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//// Ambiente de desenvolvimento
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
