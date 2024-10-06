using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuizDev.API.Filters;
using QuizDev.Application.Services;
using QuizDev.Application.UseCases.Users;
using QuizDev.Core.Repositories;
using QuizDev.Infrastructure.Data;
using QuizDev.Infrastructure.Data.Repositories;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

InjectDependencies(builder.Services);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

void InjectDependencies(IServiceCollection services)
{
    services.AddSwaggerGen(options => 
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "QuizDevAPI",
            Description = "API para desenvolvedores jogarem quizzes"
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

    services.AddControllers(options =>
    {
        options.Filters.Add<ValidateModelStateFilter>();
    });

    var connectionString = builder.Configuration.GetConnectionString("DbConnection") ?? throw new Exception("Invalid db connection");
    services.AddDbContext<QuizDevDbContext>(options =>
    {
        options.UseNpgsql(connectionString);
    });

    var jwtKey = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key") ?? throw new Exception("Invalid jwt key"));
    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {  
            IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

    //Repositories
    services.AddScoped<IUserRepository, UserRepository>();

    //Services
    services.AddScoped<AuthService>();

    //UseCases
    services.AddScoped<CreateUserUseCase>();
    services.AddScoped<LoginUserUseCase>();

}