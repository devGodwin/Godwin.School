using System.Reflection;
using AutoMapper;
using Finance.Management.Api.Configuration;
using Finance.Management.Api.Data;
using Finance.Management.Api.Services;
using Finance.Management.Api.Services.AuthServices;
using Finance.Management.Api.ServicesExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StatementOfAccountContext>(option =>
    option.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionDb")));
builder.Services.AddScoped<IStatementOfAccountServices, StatementOfAccountServices>();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<BearerTokenConfig>(builder.Configuration.GetSection(nameof(BearerTokenConfig)));
builder.Services.AddBearerAuthentication(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1",new OpenApiInfo()
    {
        Title = "Finance Management Api",
        Version = "v1",
        Description = "Finance Management Api",
        Contact = new OpenApiContact()
        {
            Name = "Godwin Mensah",
            Email = "godwinmensah945@gmail.com"
        }
    });
    x.ResolveConflictingActions(resolver=>resolver.First());
    x.EnableAnnotations();
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "Provide bearer token to access endpoints",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Scheme = "OAuth",
                Name = "Bearer",
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    x.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x=>x.SwaggerEndpoint("/swagger/v1/swagger.json","Finance Management Api"));
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();