using StoreDL;
using StoreBL;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.File("logger.txt")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// adding services for Database
builder.Services.AddScoped<ISRepo>(ctx => new DBStoreRepo(builder.Configuration.GetConnectionString("Fruits")));
builder.Services.AddScoped<ISBL, StoreStorage>();
builder.Services.AddScoped<IURepo>(ctx => new DBUserRepo(builder.Configuration.GetConnectionString("Fruits")));
builder.Services.AddScoped<IUBL, UserStorage>();

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
