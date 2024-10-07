using Microsoft.EntityFrameworkCore;
using QuizDev.Core.Entities;

namespace QuizDev.Infrastructure.Data;

public class QuizDevDbContext : DbContext
{
    public QuizDevDbContext(DbContextOptions<QuizDevDbContext> options) : base (options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Play> Plays { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionOption> QuestionOptions { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(options =>
        {
            options.HasKey(x => x.Id);
            options.HasMany(x => x.Quizes)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            options.HasMany(x => x.Plays)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            options.HasMany(x => x.Reviews)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
        });

        builder.Entity<Quiz>(options =>
        {
            options.HasKey(x => x.Id);
            options.HasMany(x => x.Questions)
                .WithOne(x => x.Quiz)
                .HasForeignKey(x => x.QuizId);

            options.HasMany(x => x.Plays)
                 .WithOne(x => x.Quiz)
                 .HasForeignKey(x => x.QuizId);
        });

        builder.Entity<Play>(options =>
        {
            options.HasKey(x => x.Id);
            options.HasOne(x => x.Review)
                .WithOne(x => x.Play)
                .HasForeignKey<Play>(x => x.ReviewId)
                .IsRequired(false);
        });

        builder.Entity<Question>(options =>
        {
            options.HasKey(x => x.Id);
            options.HasMany(x => x.Options)
                .WithOne(x => x.Question)
                .HasForeignKey(x => x.QuestionId);
        });


    }
}
