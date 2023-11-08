using System.Data.SQLite;
using Serilog;
using Driver.Services;
using Driver.Database;
using Driver.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddSingleton(new SQLiteConnection(connectionString));

var databaseInitializer = new DatabaseInitializer();

databaseInitializer.InitializeDatabase();



builder.Services.AddTransient<IDriverService, DriverService>();

builder.Services.AddTransient<ISqliteConnectionFactory, SqliteConnectionFactory>();

builder.Services.AddTransient<IDriverRepository, DriverRepository>();


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
