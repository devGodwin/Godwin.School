using System.Reflection;
using AutoMapper;
using GradeAndAssessment.Api.Data;
using GradeAndAssessment.Api.Redis;
using GradeAndAssessment.Api.Services;
using GradeAndAssessment.Api.ServicesExtension;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StudentAssessmentContext>(option =>
    option.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionDb")));

builder.Services.AddScoped<IStudentAssessmentServices, StudentAssessmentServices>();
builder.Services.AddRedisCache(c=>builder.Configuration.GetSection(nameof(RedisConfig)).Bind(c));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1",new OpenApiInfo()
    {
        Title = "Grade and Assessment Api",
        Version = "v1",
        Description = "Grade and Assessment Api",
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
    app.UseSwaggerUI(x=>x.SwaggerEndpoint("/swagger/v1/swagger.json","Grade and Assessment Api"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();