using WebBlog.Infrastructure;
using WebBlog.Infrastructure.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 
builder.Services.ConfigureSqlServerRetryOptionsPersistence(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));
builder.Services.AddSqlServerPersistence();
builder.Services.AddRepositoryPersistence();
builder.Services.AddAuthenticationWithJwt(builder.Configuration);
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
