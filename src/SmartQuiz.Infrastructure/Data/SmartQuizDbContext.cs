using Microsoft.EntityFrameworkCore;
using SmartQuiz.Core.Entities;

namespace SmartQuiz.Infrastructure.Data;

public class SmartQuizDbContext : DbContext
{
    public SmartQuizDbContext(DbContextOptions<SmartQuizDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<AnswerOption> AnswerOptions { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Response> Responses { get; set; }
    public DbSet<EmailCode> EmailCodes { get; set; }

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
        });

        builder.Entity<Question>(options =>
        {
            options.HasKey(x => x.Id);
            options.HasMany(x => x.Options)
                .WithOne(x => x.Question)
                .HasForeignKey(x => x.QuestionId);
        });

        builder.Entity<Response>(options =>
        {
            options.HasKey(x => x.Id);
            options.HasOne(x => x.Match)
                .WithMany(x => x.Responses)
                .HasForeignKey(x => x.MatchId);

            options.HasOne(x => x.AnswerOption)
                .WithMany()
                .HasForeignKey(x => x.AnswerOptionId);
        });

        builder.Entity<Review>(options =>
        {
            options.HasKey(x => x.Id);
            options.HasOne(x => x.Match)
                .WithOne(x => x.Review)
                .HasForeignKey<Match>(x => x.ReviewId)
                .OnDelete(DeleteBehavior.SetNull);

            options.HasOne(x => x.Quiz)
                .WithMany();
        });

        builder.Entity<EmailCode>(options =>
        {
            options.HasKey(x => x.Code);
            options.HasIndex(x => x.Email).IsUnique(true);
        });
    }
}
