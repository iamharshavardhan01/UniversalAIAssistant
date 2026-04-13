using DigiSoft.Database.Entities.CommonFields;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversalAiAssistant.Domain.Entities
{
    [Table("chat_message")]
    public class ChatMessage : PrimaryKey
    {
        [Column("role")]
        public string Role { get; set; } = string.Empty;

        [Column("content")]
        public string Content { get; set; } = string.Empty;

        [Column("time_stamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Column("chat_session_id")]
        public long ChatSessionId { get; set; }

        [ForeignKey(nameof(ChatSessionId))]
        [InverseProperty(nameof(ChatSession.ChatMessages))]
        public ChatSession ChatSession { get; set; } = null!;
    }
}
