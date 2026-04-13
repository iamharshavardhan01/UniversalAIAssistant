using DigiSoft.Database.Entities.CommonFields;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using UniversalAIAssistant.Domain.Entities;

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
        public DateTime StartedAt { get; set; }

        [Column("chat_bot_id")]
        public long ChatBotId { get; set; }

        [ForeignKey(nameof(ChatBotId))]
        [InverseProperty("ChatSessions")]
        public ChatBot ChatBot { get; set; } = null!;

        [InverseProperty("ChatSessions")]
        public ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
