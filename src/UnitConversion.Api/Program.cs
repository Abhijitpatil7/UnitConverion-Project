using UnitConversion.Business.Services;
using UnitConversion.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<IConversionService, ConversionService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(); // Swagger UI at /swagger

app.MapControllers();
app.Run();
