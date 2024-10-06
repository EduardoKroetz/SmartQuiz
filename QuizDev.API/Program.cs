using Microsoft.EntityFrameworkCore;
using QuizDev.Application.Services;
using QuizDev.Application.UseCases.Users;
using QuizDev.Core.Repositories;
using QuizDev.Infrastructure.Data;
using QuizDev.Infrastructure.Data.Repositories;

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
    services.AddSwaggerGen();
    services.AddControllers();

    var connectionString = builder.Configuration.GetConnectionString("DbConnection") ?? throw new Exception("Invalid db connection");
    services.AddDbContext<QuizDevDbContext>(options =>
    {
        options.UseNpgsql(connectionString);
    });

    //Repositories
    services.AddScoped<IUserRepository, UserRepository>();

    //Services
    services.AddScoped<AuthService>();

    //UseCases
    services.AddScoped<CreateUserUseCase>();

}