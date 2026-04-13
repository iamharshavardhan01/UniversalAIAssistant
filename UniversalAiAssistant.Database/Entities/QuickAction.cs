using DigiSoft.Database.Entities.CommonFields;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversalAiAssistant.Domain.Entities
{
    [Table("quick_action")]
    public class QuickAction : PrimaryKey
    {
        [Column("action_name")]
        public string ActionName { get; set; } = string.Empty;

        [Column("action_url")]
        public string ActionUrl { get; set; } = string.Empty;

        [Column("display_order")]
        public int DisplayOrder { get; set; }


        [Column("chat_bot_id")]
        public long ChatBotId { get; set; }

        [ForeignKey(nameof(ChatBotId))]
        [InverseProperty("QuickActions")]
        public ChatBot ChatBot { get; set; } = null!;
    }
}
