using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Services.Implementations;
using ProductApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

// Configurar DbContext con MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("MySql");
    options.UseMySql(cs, ServerVersion.AutoDetect(cs));
});

// Registrar servicios
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();

var app = builder.Build();

// Probar conexión a la base de datos
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        if (await db.Database.CanConnectAsync())
            Console.WriteLine("Conexión a MySQL exitosa");
        else
            Console.WriteLine("No se pudo conectar a MySQL");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error al conectar con MySQL");
        Console.WriteLine(ex.Message);
    }
}

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Middleware
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
