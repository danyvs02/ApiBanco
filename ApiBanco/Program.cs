using ApiBanco.Data;
using ApiBanco.Mappers;
using ApiBanco.Repositorio;
using ApiBanco.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


//2. Configuramos la conexion a sql server
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
{
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSqlBanco"));
});


// Add services to the container.

builder.Services.AddScoped<ICuentaRepositorio, CuentaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<ITransaccionRepositorio, TransaccionRepositorio>();

//Agregar Automapper
builder.Services.AddAutoMapper(typeof(BancoMapper));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 5. Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        // Permite solicitudes de un origen específico
        policy.AllowAnyOrigin()   // Permite cualquier origen
              .AllowAnyHeader()   // Permite cualquier encabezado
              .AllowAnyMethod();   // Permite cualquier método HTTP
    });
});


// Registrar servicios específicos
builder.Services.AddScoped<Notificador>();  // Registrar el servicio Notificador
builder.Services.AddScoped<VoucherService>(); // Registrar el servicio VoucherService

var app = builder.Build();


// Configurar la tubería de solicitudes HTTP.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();          // Habilitar Swagger en todos los entornos
    app.UseSwaggerUI();       // Habilitar la interfaz de Swagger
}

app.UseHttpsRedirection();

// 8. Aplicar la política CORS
app.UseCors("AllowSpecificOrigin");  // Aplica la política CORS definida arriba

app.UseAuthorization();

app.MapControllers();

app.Run();
