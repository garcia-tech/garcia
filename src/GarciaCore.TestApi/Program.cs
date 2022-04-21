using GarciaCore.Application.Contracts.Identity;
using GarciaCore.Infrastructure.FileUpload.AmazonS3;
using GarciaCore.Infrastructure.Identity;
using GarciaCore.Infrastructure.ImageResize.Local;
using GarciaCore.TestApi.Controllers;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddJwtOptions(builder.Configuration);
builder.Services.AddLocalImageResizeService(builder.Configuration);
builder.Services.AddAmazonS3FileUploadService(builder.Configuration);
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
