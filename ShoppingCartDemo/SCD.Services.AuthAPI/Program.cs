using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SCD.Services.AuthAPI.Data;
using SCD.Services.AuthAPI.Models;
using SCD.Services.AuthAPI.Service;
using SCD.Services.AuthAPI.Service.IService;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);


/// Get the Key Vault URI
string keyVaultUrl = "https://scd-sql-key-vault.vault.azure.net/";

// Create a SecretClient using DefaultAzureCredential
var secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

// Retrieve the secret
KeyVaultSecret secret = secretClient.GetSecret("auth-db-connectionstring");
string connectionString = secret.Value;
//Console.WriteLine(connectionString);
// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
options.UseSqlServer(connectionString); //builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow_SCD_UI", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetValue<string>("AllowedOrigins").Split(","))
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Allow_SCD_UI");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
ApplyMigration();
app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}