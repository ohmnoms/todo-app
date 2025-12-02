using Todo.Api.Models.TodoDb;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddControllers();


builder.Services.AddDbContext<TodoDbContext>(
    opts => opts.UseInMemoryDatabase("TodoDb")
        .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseHealthChecks("/health");

app.Run();