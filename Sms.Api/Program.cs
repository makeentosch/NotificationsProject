using NotificationServiceDefaults;
using Sms.Application;
using Sms.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.AddInfrastructure();
builder.AddLogic();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.MapDefaultEndpoints();

app.Run();
