using Garcia.Application.Contracts.PushNotification;
using Garcia.Infrastructure.PushNotification.Firebase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ################ Garcia Code Start ################
// Inject IPushNotificationService
builder.Services.AddScoped<IPushNotificationService, FirebasePushNotificationService>();

// Inject FirebasePushNotificationSettings
builder.Services.Configure<FirebasePushNotificationSettings>(builder.Configuration.GetSection("FirebasePushNotificationSettings"));
// ################ Garcia Code End ################

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
