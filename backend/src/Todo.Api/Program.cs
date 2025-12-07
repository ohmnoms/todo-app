using Microsoft.EntityFrameworkCore;
using Todo.Api.Services;
using Todo.Api.Persistence;
using Todo.Api.Common;
using Microsoft.OpenApi;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHealthChecks();
builder.Services.AddControllers();

// Swagger docs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Todo.Api",
        Version = "v1",
        Description = "Simple Todo API for the Vue 'Do It' app."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }
    else
    {
        // Temporary: helps confirm path issues while debugging
        Console.WriteLine($"XML comments file not found: {xmlPath}");
    }
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy
            .WithOrigins("http://localhost:5173") // Vite dev URL
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Dependency Injection
builder.Services.AddScoped<ITodoItemsRepository, TodoItemsRepository>();
builder.Services.AddScoped<ITodoItemsService, TodoItemsService>();

builder.Services.AddDbContext<TodoDbContext>(
    options => options.UseInMemoryDatabase("TodoDb")
        .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
);

//Persisted SQLite database configuration
builder.Services.AddDbContext<TodoDbContext>(options =>
{   
    options.UseSqlite(builder.Configuration.GetConnectionString("TodoDb"))
        .EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo.Api v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseMiddleware<ExceptionHandler>();
app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();
app.UseHealthChecks("/health");

app.Run();