using Microsoft.EntityFrameworkCore;
using MiniLedger.Api.Data;
using MiniLedger.Api.Repositories.Implementations;
using MiniLedger.Api.Repositories.Interfaces;
using MiniLedger.Api.Services.Implementations;
using MiniLedger.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'Default' is not configured.");

var serverVersion = new MySqlServerVersion(new Version(8, 0, 32));

builder.Services.AddDbContext<MiniLedgerDbContext>(dbContextOptions =>
{
    dbContextOptions.UseMySql(connectionString, serverVersion, mysqlOptions =>
    {
        mysqlOptions.EnableRetryOnFailure();
        // mysqlOptions.CommandTimeout(60);
    });

    if (builder.Environment.IsDevelopment())
    {
        dbContextOptions
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }

});

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();