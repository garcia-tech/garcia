using Garcia.Application;
using Garcia.Infrastructure.Api;
using Garcia.Infrastructure.Identity;
using Garcia.Persistence.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuthorization();
builder.Services
    .AddEfCoreInMemory<BaseContext>("Sample")
    .AddEfCoreRepository();

builder.Services.AddBaseService<Sample, SampleDto>();
builder.Services.AddAuthenticationService<User, UserDto>(builder.Configuration);

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
