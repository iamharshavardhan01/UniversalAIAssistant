using DigiSoft.Database.Entities.CommonFields;
using System.ComponentModel.DataAnnotations.Schema;
using UniversalAiAssistant.Domain.Entities;

namespace UniversalAIAssistant.Domain.Entities;

[Table("chat_message")]
public class ChatMessage : PrimaryKey
{
    [Column("role")]
    public string Role { get; set; } = string.Empty; // "user" or "assistant"

    [Column("content")]
    public string Content { get; set; } = string.Empty;

    [Column("time_stamp")]
    public DateTime Timestamp { get; set; }

    [Column("chat_session_id")]
    public long ChatSessionId { get; set; }

    [ForeignKey(nameof(ChatSessionId))]
    [InverseProperty("ChatMessage")]
    public ChatSession ChatSession { get; set; } = null!;

}