using Microsoft.EntityFrameworkCore;
using QuizDev.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DbConnection") ?? throw new Exception("Invalid db connection");
builder.Services.AddDbContext<QuizDevDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.UseHttpsRedirection();

app.Run();