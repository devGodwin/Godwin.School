using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Student.Management.Api.Data;
using Student.Management.Api.Redis;
using Student.Management.Api.Services;
using Student.Management.Api.ServicesExtension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StudentContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionDb")));
builder.Services.AddScoped<IStudentServices, StudentServices>();
builder.Services.AddRedisCache(c=>builder.Configuration.GetSection(nameof(RedisConfig)).Bind(c));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x=>
{
    x.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Student Management Api",
        Version = "v1",
        Description = "Student Management Api",
        Contact = new OpenApiContact()
        {
            Name = "Godwin Mensah",
            Email = "godwinmensah945@gmail.com"
        }
    });
    x.ResolveConflictingActions(resolver=>resolver.First());
    x.EnableAnnotations();

    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    x.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x=>x.SwaggerEndpoint("/swagger/v1/swagger.json","Student Management Api"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();