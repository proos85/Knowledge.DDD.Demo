using System.Reflection;
using Knowledge.DDD.Demo.Core.Contracts;
using Knowledge.DDD.Demo.Core.Domain;
using Knowledge.DDD.Demo.Core.Services;
using Knowledge.DDD.Demo.Infra.Messages;
using Knowledge.DDD.Demo.Infra.Payment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments($@"{AppDomain.CurrentDomain.BaseDirectory}\Knowledge.DDD.Demo.WebApi.xml");
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.RegisterServices();
builder.Services.RegisterContracts();
builder.Services.RegisterDomain();
builder.Services.RegisterInfraPayment();

SnackOrderCompleteEventReceiver.Start();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
