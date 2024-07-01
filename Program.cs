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

// Buscando conex�o com database para as tabelas das entidades
var entidadesConnectionString = builder.Configuration.GetConnectionString("EntidadesConnection");

builder.Services.AddDbContext<EntidadesContext>(opt =>
    opt.UseLazyLoadingProxies().UseMySql(entidadesConnectionString, ServerVersion.AutoDetect(entidadesConnectionString)));

// Buscando conex�o com database para as tabelas de usu�rios
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

// Configura��o de Password de usu�rio
builder.Services.Configure<IdentityOptions>(options =>
{
    // Define o comprimento m�nimo necess�rio para a senha
    options.Password.RequiredLength = 10;

    // Exige que a senha contenha pelo menos um d�gito num�rico (0-9)
    options.Password.RequireDigit = true;

    // Exige que a senha contenha pelo menos uma letra min�scula (a-z)
    options.Password.RequireLowercase = true;

    // Define que n�o � necess�rio que a senha contenha caracteres n�o alfanum�ricos (como @, #, $)
    options.Password.RequireNonAlphanumeric = false;

    // Exige que a senha contenha pelo menos uma letra mai�scula (A-Z)
    options.Password.RequireUppercase = true;

    // Especifica o n�mero m�nimo de caracteres �nicos que a senha deve conter
    options.Password.RequiredUniqueChars = 1;
});

// Gera uma chave forte para assinar o token JWT
// (Sempre que o servidor for iniciado, gera uma nova chave)
var securityKey = new byte[32]; // 32 bytes = 256 bits
using (var generator = RandomNumberGenerator.Create())
{
    generator.GetBytes(securityKey);
}

// Configurar a chave de seguran�a para o JWT
builder.Services.AddSingleton(new SymmetricSecurityKey(securityKey));

// Adicionando servi�o de autentica��o por JWT 
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

// Adicionando autoriza��es de acesso aos dados
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

// Configurando prote��o de dados
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/app/ExternalDataProtectionKeys"))
    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
    {
        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
    });

// Adiciona a pol�tica de CORS
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
    // Define a URL base para o HttpClient. Esta � a parte comum da URL para todas as solicita��es feitas por este cliente.
    client.BaseAddress = new Uri("https://password-generator-by-api-ninjas.p.rapidapi.com/v1/");

    // Adiciona o cabe�alho "x-rapidapi-key" com a chave de API necess�ria para autenticar as solicita��es.
    // Esta chave deve ser mantida em segredo e n�o deve ser exposta publicamente.
    client.DefaultRequestHeaders.Add("x-rapidapi-key", "f3d3fd1e1bmsh0bbecb399458a08p1c3e13jsn99e07e0be825");

    // Adiciona o cabe�alho "x-rapidapi-host" com o host da API. Este cabe�alho � utilizado pela API para roteamento e verifica��o.
    client.DefaultRequestHeaders.Add("x-rapidapi-host", "password-generator-by-api-ninjas.p.rapidapi.com");
});


var app = builder.Build();

// Usa a pol�tica de CORS
app.UseCors("CorsPolicy");

// Ambiente de produ��o
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
