using System.Collections.Concurrent;
using hello_api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IBackgroundTaskQueue , BackgroundTaskQueue>();

builder.Services.AddTransient<WorkItem>();
builder.Services.AddScoped<Func<WorkItem>>(
services => () => services.GetService<WorkItem>()!); 

builder.Services.AddHostedService<TaskProcessorService>();


builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();


