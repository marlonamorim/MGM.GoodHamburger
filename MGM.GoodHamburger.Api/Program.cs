using MGM.GoodHamburger.Application.DependencyInjection;
using MGM.GoodHamburger.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Infrastructure layer (DbContext + Repositories)
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application layer (MediatR + Handlers)
builder.Services.AddApplication();

builder.Services.AddControllers();

// Add Swagger/OpenAPI
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

app.UseAuthorization();

app.MapControllers();

app.Run();
