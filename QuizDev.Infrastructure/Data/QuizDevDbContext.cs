using Microsoft.EntityFrameworkCore;
using QuizDev.Core.Entities;

namespace QuizDev.Infrastructure.Data;

public class QuizDevDbContext : DbContext
{
    public QuizDevDbContext(DbContextOptions<QuizDevDbContext> options) : base (options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionOption> QuestionOptions { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<MatchResponse> MatchResponses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(options =>
        {
            options.HasKey(x => x.Id);
            options.HasMany(x => x.Quizes)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            options.HasMany(x => x.Matchs)
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

            options.HasMany(x => x.Matchs)
                 .WithOne(x => x.Quiz)
                 .HasForeignKey(x => x.QuizId);
        });

        builder.Entity<Match>(options =>
        {
            options.HasKey(x => x.Id);
            options.HasOne(x => x.Review)
                .WithOne(x => x.Match)
                .HasForeignKey<Match>(x => x.ReviewId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<Question>(options =>
        {
            options.HasKey(x => x.Id);
            options.HasMany(x => x.Options)
                .WithOne(x => x.Question)
                .HasForeignKey(x => x.QuestionId);
        });

        builder.Entity<MatchResponse>(options =>
        {
            options.HasKey(x => x.Id);
            options.HasOne(x => x.Match)
                .WithMany(x => x.Responses)
                .HasForeignKey(x => x.MatchId);

            options.HasOne(x => x.QuestionOption)
                .WithMany()
                .HasForeignKey(x => x.QuestionOptionId);
        });


    }
}
