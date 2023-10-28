using System.Data.SQLite;
using Driver;
using Serilog;
using Driver.Services;
using Driver.Database;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//SQLite

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

// set log to file 


builder.Services.AddTransient<IDriverService, DriverService>();

builder.Services.AddTransient<ISqliteConnectionFactory, SqliteConnectionFactory>();




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
