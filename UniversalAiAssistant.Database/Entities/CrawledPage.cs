using DigiSoft.Database.Entities.CommonFields;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversalAiAssistant.Domain.Entities
{
    [Table("crawled_page")]
    public class CrawledPage : PrimaryKey
    {
        [Column("url")]
        public string Url { get; set; } = string.Empty;

        [Column("content")]
        public string Content { get; set; } = string.Empty;

        [Column("source_type")]
        public string SourceType { get; set; } = "website";

        [Column("last_crawled")]
        public DateTime LastCrawled { get; set; }

        [Column("chat_bot_id")]
        public long ChatBotId { get; set; }

        [ForeignKey(nameof(ChatBotId))]
        [InverseProperty("CrawledPages")]
        public ChatBot ChatBot { get; set; } = null!;
    }
}
