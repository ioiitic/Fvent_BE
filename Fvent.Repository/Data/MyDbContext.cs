using Fvent.BO.Entities;
using Fvent.Repository.Interceptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fvent.Repository.Data;

public class MyDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventTag> EventTags { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<EventFile> EventFiles { get; set; }
    public DbSet<EventType> EventTypes { get; set; }
    public DbSet<EventRegistration> EventRegistrations { get; set; }
    public DbSet<EventReview> EventReviews { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<EventMedia> EventMedia { get; set; }
    public DbSet<EventFollower> EventFollowers { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<VerificationToken> VerificationTokens { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Form> Forms { get; set; }
    public DbSet<FormDetail> FormDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            //Note: Remove this to migrationdotnet
            //.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Fvent.API/"))
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        optionsBuilder
            .UseSqlServer(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .AddInterceptors(new SoftDeleteInterceptor());
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasQueryFilter(u => !u.IsDeleted && u.EmailVerified);

        modelBuilder.Entity<Event>()
            .HasQueryFilter(u => !u.IsDeleted);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EventFollower>()
            .HasOne(c => c.User)
            .WithMany(u => u.Followers)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EventRegistration>()
            .HasOne(c => c.User)
            .WithMany(u => u.Registrations)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EventReview>()
            .HasOne(c => c.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Notification>()
            .HasOne(c => c.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
            .HasOne(c => c.Sender)
            .WithMany(u => u.Messages)
            .HasForeignKey(c => c.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<VerificationToken>()
                    .HasKey(vt => vt.UserId);
    }
}
