using Microsoft.EntityFrameworkCore;
using Reddit;
using Reddit.ActionFilters;
using Reddit.Mapper;
using Reddit.Middlewares;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => {
    options.Filters.Add<LogActionFilter>();
    options.Filters.Add<ValidateModelAttribute>();
    })
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplcationDBContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteDb"));
    options.UseLazyLoadingProxies();
    options.UseLoggerFactory(LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
        builder.SetMinimumLevel(LogLevel.Information);
    }));
});


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
               builder => builder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader());
});
builder.Services.AddSingleton<IMapper, Mapper>();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ExceptionHandler2>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseExceptionHandler();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
     await next();
});

app.Use(async (context, next) =>
{

    if (context.Request.Headers.ContainsKey("X-Stop-Middleware"))
    {
        // Optionally, you can write a response here if needed
        context.Response.StatusCode = 403; // For example, Forbidden status code
        await context.Response.WriteAsync("Middleware execution stopped.");
    }
    else
    {
        await next();
    }
});



app.MapGet("/throw", () =>
{
    throw new Exception("sample ex");
})
.WithName("throw")
.WithOpenApi();


app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();

