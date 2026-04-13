using Microsoft.EntityFrameworkCore;
using UniversalAIAssistant.Domain.Entities;

namespace UniversalAiAssistant.Domain.Entities
{
    public class AppContext : DbContext
    {
        public AppContext()
        {
        }

        public AppContext(DbContextOptions<AppContext> dbContextOptions) : base(dbContextOptions)
        {

        }


        public virtual DbSet<ChatBot> ChatBots { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<ChatSession> ChatSessions { get; set; }

        public virtual DbSet<QuickAction> QuickActions { get; set; }

        public virtual DbSet<CrawledPage> CrawledPages { get; set; }


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

            // Convert enum to string
            modelBuilder.Entity<ChatBot>()
                .Property(c => c.ModelName)
                .HasConversion<string>();

            // modelBuilder.SeedData();
        }
    }
}
