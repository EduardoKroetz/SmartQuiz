using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartQuiz.API.Filters;
using SmartQuiz.API.Middlewares;
using SmartQuiz.Application.Services;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Application.UseCases.AnswerOptions;
using SmartQuiz.Application.UseCases.Matches;
using SmartQuiz.Application.UseCases.Questions;
using SmartQuiz.Application.UseCases.Quizzes;
using SmartQuiz.Application.UseCases.Responses;
using SmartQuiz.Application.UseCases.Reviews;
using SmartQuiz.Application.UseCases.Users;
using SmartQuiz.Core.DTOs.Questions;
using SmartQuiz.Core.Repositories;
using SmartQuiz.Infrastructure.Data;
using SmartQuiz.Infrastructure.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

InjectDependencies(builder.Services);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsProduction() || app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<SmartQuizDbContext>();
        dbContext.Database.Migrate();
    }
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();

app.Run();

void InjectDependencies(IServiceCollection services)
{
    services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "SmartQuizAPI",
            Description = "API para criar, gerar e jogar quizzes"
        });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Description = "Coloque SOMENTE o token JWT",
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

    services.AddControllers(options =>
    {
        options.Filters.Add<ValidateModelStateFilter>();
    });

    if (services.SingleOrDefault(x => x.ServiceType == typeof(SmartQuizDbContext)) == null)
    {
        var connectionString = builder.Configuration.GetConnectionString("DbConnection") ?? throw new Exception("Invalid db connection");
        services.AddDbContext<SmartQuizDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
    }

    var jwtKey = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key") ?? throw new Exception("Invalid jwt key"));
    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

    services.AddProblemDetails();
    services.AddExceptionHandler<ExceptionHandler>();

    //Repositories
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IQuizRepository, QuizRepository>();
    services.AddScoped<IQuestionRepository, QuestionRepository>();
    services.AddScoped<IAnswerOptionRepository, AnswerOptionRepository>();
    services.AddScoped<IMatchRepository, MatchRepository>();
    services.AddScoped<IResponseRepository, ResponseRepository>();
    services.AddScoped<IReviewRepository, ReviewRepository>();
    services.AddScoped<IEmailCodeRepository, EmailCodeRepository>();

    //Services
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IEmailService, EmailService>();

    //UseCases
    services.AddScoped<CreateUserUseCase>();
    services.AddScoped<LoginUserUseCase>();
    services.AddScoped<GetUserUseCase>();
    services.AddScoped<GetUserMatchesUseCase>();
    services.AddScoped<GetUserQuizzesUseCase>();

    services.AddScoped<CreateQuizUseCase>();
    services.AddScoped<GetQuizByIdUseCase>();
    services.AddScoped<SearchQuizUseCase>();
    services.AddScoped<SearchQuizByReviewsUseCase>();
    services.AddScoped<ToggleQuizUseCase>();
    services.AddScoped<UpdateQuizUseCase>();
    services.AddScoped<DeleteQuizUseCase>();
    services.AddScoped<GetQuestionsByQuizUseCase>();

    services.AddScoped<CreateQuestionUseCase>();
    services.AddScoped<GetQuestionDetailsUseCase>();
    services.AddScoped<UpdateQuestionUseCase>();
    services.AddScoped<DeleteQuestionUseCase>();
    services.AddScoped<UpdateCorrectOptionUseCase>();

    services.AddScoped<CreateMatchUseCase>();
    services.AddScoped<CreateResponseUseCase>();
    services.AddScoped<GetNextQuestionUseCase>();
    services.AddScoped<GetMatchUseCase>();
    services.AddScoped<FinalizeMatchUseCase>();
    services.AddScoped<GetResponsesByMatchUseCase>();
    services.AddScoped<DeleteMatchUseCase>();
    services.AddScoped<GetMatchesUseCase>();

    services.AddScoped<CreateAnswerOptionUseCase>();
    services.AddScoped<DeleteAnswerOptionUseCase>();
    services.AddScoped<GetAnswerOptionsByQuestionUseCase>();

    services.AddScoped<CreateReviewUseCase>();
    services.AddScoped<DeleteReviewUseCase>();
    services.AddScoped<UpdateReviewUseCase>();
    services.AddScoped<GetReviewDetailsUseCase>();

    services.AddScoped<VerifyEmailUseCase>();
    services.AddScoped<VerifyEmailCodeUseCase>();
}

public class Startup { }