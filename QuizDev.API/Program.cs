using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuizDev.API.Filters;
using QuizDev.Application.Services;
using QuizDev.Application.UseCases.Matches;
using QuizDev.Application.UseCases.Responses;
using QuizDev.Application.UseCases.Questions;
using QuizDev.Application.UseCases.Quizzes;
using QuizDev.Application.UseCases.Users;
using QuizDev.Core.Repositories;
using QuizDev.Infrastructure.Data;
using QuizDev.Infrastructure.Data.Repositories;
using System.Reflection;
using System.Text;
using QuizDev.Core.DTOs.Questions;
using QuizDev.Core.DTOs.AnswerOptions;
using QuizDev.Application.UseCases.AnswerOptions;
using QuizDev.Application.UseCases.Reviews;

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
    services.AddScoped<IQuizRepository, QuizRepository>();
    services.AddScoped<IQuestionRepository, QuestionRepository>();
    services.AddScoped<IAnswerOptionRepository, AnswerOptionRepository>();
    services.AddScoped<IMatchRepository, MatchRepository>();
    services.AddScoped<IResponseRepository, ResponseRepository>();
    services.AddScoped<IReviewRepository, ReviewRepository>();

    //Services
    services.AddScoped<AuthService>();

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
}