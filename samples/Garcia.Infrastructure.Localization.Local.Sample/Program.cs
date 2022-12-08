using Garcia.Infrastructure.Localization.Local;
using Garcia.Infrastructure.Localization.Local.Sample;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ################ Garcia Code Start ################
// builder.Services.AddGarciaLocalization<LocalizationSampleDbContext>(options => options.UseInMemoryDatabase("LocalizationSample"));
builder.Services.AddGarciaLocalization<LocalizationSampleDbContext>("LocalizationSample");
// ################ Garcia Code End ################

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
