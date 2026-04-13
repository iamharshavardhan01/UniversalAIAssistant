using Microsoft.EntityFrameworkCore;

namespace UniversalAiAssistant.Domain.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public virtual DbSet<ChatBot> ChatBots => Set<ChatBot>();
        public virtual DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
        public virtual DbSet<ChatSession> ChatSessions => Set<ChatSession>();
        public virtual DbSet<QuickAction> QuickActions => Set<QuickAction>();
        public virtual DbSet<CrawledPage> CrawledPages => Set<CrawledPage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatBot>()
                .HasMany(c => c.QuickActions)
                .WithOne(q => q.ChatBot)
                .HasForeignKey(q => q.ChatBotId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChatBot>()
                .HasMany(c => c.CrawledPages)
                .WithOne(cp => cp.ChatBot)
                .HasForeignKey(cp => cp.ChatBotId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChatBot>()
                .HasMany(c => c.ChatSessions)
                .WithOne(cs => cs.ChatBot)
                .HasForeignKey(cs => cs.ChatBotId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChatSession>()
                .HasMany(cs => cs.ChatMessages)
                .WithOne(m => m.ChatSession)
                .HasForeignKey(m => m.ChatSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChatBot>()
                .Property(c => c.ModelName)
                .HasConversion<string>();

            modelBuilder.Entity<ChatBot>()
                .Property(c => c.ProjectName)
                .HasMaxLength(200);

            modelBuilder.Entity<ChatBot>()
                .Property(c => c.ProjectUrl)
                .HasMaxLength(1000);

            modelBuilder.Entity<ChatSession>()
                .HasIndex(x => x.SessionToken)
                .IsUnique();
        }
    }
}
