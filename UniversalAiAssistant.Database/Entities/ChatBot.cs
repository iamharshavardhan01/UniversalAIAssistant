using DigiSoft.Common.Global;
using DigiSoft.Database.Entities.CommonFields;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversalAiAssistant.Domain.Entities
{
    [Table("chat_bot")]
    public class ChatBot : IdAndDatesWithIsActive
    {
        public ChatBot()
        {
            QuickActions = new HashSet<QuickAction>();
            CrawledPages = new HashSet<CrawledPage>();
            ChatSessions = new HashSet<ChatSession>();
        }

        [Column("project_name")]
        public string ProjectName { get; set; } = string.Empty;

        [Column("project_url")]
        public string ProjectUrl { get; set; } = string.Empty;

        [Column("model_name")]
        public AIChatModel ModelName { get; set; }

        [Column("encrypted_api_key")]
        public string EncryptedApiKey { get; set; } = string.Empty;

        [Column("encrypted_db_connection_string")]
        public string EncryptedDbConnectionString { get; set; } = string.Empty;

        [Column("background_color")]
        public string BackgroundColor { get; set; } = "#FFFFFF";

        [Column("foreground_color")]
        public string ForegroundColor { get; set; } = "#000000";

        [Column("text_color")]
        public string TextColor { get; set; } = "#333333";

        [Column("crawl_depth")]
        public int CrawlDepth { get; set; } = 2;

        [Column("last_trained_at")]
        public DateTime? LastTrainedAt { get; set; }

        [InverseProperty(nameof(QuickAction.ChatBot))]
        public ICollection<QuickAction> QuickActions { get; set; }

        [InverseProperty(nameof(CrawledPage.ChatBot))]
        public ICollection<CrawledPage> CrawledPages { get; set; }

        [InverseProperty(nameof(ChatSession.ChatBot))]
        public ICollection<ChatSession> ChatSessions { get; set; }
    }
}
