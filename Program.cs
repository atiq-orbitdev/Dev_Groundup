using Microsoft.EntityFrameworkCore;
using Dev_Groundup.Data;
using Dev_Groundup.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add ToDoContext to services
builder.Services.AddDbContext<ToDoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add UserContext to services
builder.Services.AddDbContext<UserContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

// Add API endpoints for ToDoItem
app.MapPost("/todo", async (ToDoContext context, ToDoItem toDoItem) =>
{
    if (context.ToDoItem == null)
    {
        return Results.Problem("ToDoItems collection is not initialized.");
    }
    context.ToDoItem.Add(toDoItem);
    await context.SaveChangesAsync();
    return Results.Created($"/todo/{toDoItem.Id}", toDoItem);
});

app.MapPut("/todo/{id}", async (ToDoContext context, int id, ToDoItem updatedToDoItem) =>
{
    if (context.ToDoItem == null)
    {
        return Results.Problem("ToDoItems collection is not initialized.");
    }
    var toDoItem = context.ToDoItem == null ? null : await context.ToDoItem.FindAsync(id);
    if (toDoItem == null)
    {
        return Results.NotFound();
    }

    toDoItem.Title = updatedToDoItem.Title;
    toDoItem.IsComplete = updatedToDoItem.IsComplete;

    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/todo/{id}", async (ToDoContext context, int id) =>
{
    if (context.ToDoItem == null)
    {
        return Results.Problem("ToDoItems collection is not initialized.");
    }

    var toDoItem = await context.ToDoItem.FindAsync(id);
    if (toDoItem is null)
    {
        return Results.NotFound();
    }

    context.ToDoItem.Remove(toDoItem!);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/todo/incomplete", async (ToDoContext context) =>
{
    if (context.ToDoItem == null)
    {
        return Results.Problem("ToDoItems collection is not initialized.");
    }

    var incompleteItems = await context.ToDoItem
        .Where(item => !item.IsComplete)
        .ToListAsync();

    return Results.Ok(incompleteItems);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
