using Microsoft.EntityFrameworkCore;
using DebtManagement.Infrastructure.Data;
using DebtManagement.Domain.Interfaces;
using DebtManagement.Infrastructure.Repositories;
using DebtManagement.Application.Services;
using DebtManagement.Application.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do Database com InMemory
builder.Services.AddDbContext<DebtDbContext>(options =>
    options.UseInMemoryDatabase("DebtManagementDB"));

// Dependency Injection
builder.Services.AddScoped<IDebtRepository, DebtRepository>();
builder.Services.AddScoped<IDebtService, DebtService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthorization();
app.MapControllers();

// Criar database em memória
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DebtDbContext>();
    context.Database.EnsureCreated();
}

app.Run();