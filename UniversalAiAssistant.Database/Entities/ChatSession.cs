using DigiSoft.Database.Entities.CommonFields;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversalAiAssistant.Domain.Entities
{
    [Table("chat_session")]
    public class ChatSession : PrimaryKey
    {
        public ChatSession()
        {
            ChatMessages = new HashSet<ChatMessage>();
        }

        [Column("started_at")]
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        [Column("session_token")]
        public string SessionToken { get; set; } = Guid.NewGuid().ToString("N");

        [Column("chat_bot_id")]
        public long ChatBotId { get; set; }

        [ForeignKey(nameof(ChatBotId))]
        [InverseProperty(nameof(ChatBot.ChatSessions))]
        public ChatBot ChatBot { get; set; } = null!;

        [InverseProperty(nameof(ChatMessage.ChatSession))]
        public ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
