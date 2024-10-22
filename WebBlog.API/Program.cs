using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebBlog.API.Middlewares;
using WebBlog.Infrastructure;
using WebBlog.Infrastructure.DependencyInjection.Extensions;
using WebBlog.Infrastructure.Identity;
using WebBlog.Infrastructure.Persistance.DataSeeding;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// 
builder.Services.AddSwagger();

builder.Services.ConfigureSqlServerRetryOptionsPersistence(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));
builder.Services.AddSqlServerPersistence();
builder.Services.AddRepositoryPersistence();
builder.Services.AddAuthenticationWithJwt(builder.Configuration);
builder.Services.AddAuthorization();


var app = builder.Build();

// Seed roles
var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
//try
//{
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await RoleSeeder.SeedRolesAsync(roleManager);
//}
//catch
//{
//    var logger = services.GetRequiredService<ILogger<Program>>();
//    logger.LogError("Seed failed");
//}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
