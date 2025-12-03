using Microsoft.EntityFrameworkCore;
using Todo.Api.Services;
using Todo.Api.Persistence;
using Todo.Api.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddControllers();

// Dependency Injection
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddDbContext<TodoDbContext>(
    opts => opts.UseInMemoryDatabase("TodoDb")
        .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
);

// Persisted SQLite database configuration
// builder.Services.AddDbContext<TodoDbContext>(options =>
//     options.UseSqlite(builder.Configuration.GetConnectionString("TodoDb")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseMiddleware<ExceptionHandler>();
app.UseHttpsRedirection();
app.MapControllers();
app.UseHealthChecks("/health");

app.Run();