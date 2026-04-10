using UserManagementAPI.Middleware;
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 1️⃣ Error Handling FIRST
app.UseMiddleware<ErrorHandlingMiddleware>();

// 2️⃣ Authentication
app.UseMiddleware<AuthMiddleware>();

// 3️⃣ Logging LAST
app.UseMiddleware<LoggingMiddleware>();

app.MapControllers();

app.Run();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();