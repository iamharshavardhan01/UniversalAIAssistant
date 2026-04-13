using DigiSoft.Common.Global;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalAiAssistant.Shared.Models
{
    public class QuickActionModel
    {
        [JsonProperty("actionName")]
        public string ActionName { get; set; } = string.Empty;

        [JsonProperty("actionUrl")]
        public string ActionUrl { get; set; } = string.Empty;

        [JsonProperty("displayOrder")]
        public int DisplayOrder { get; set; }
    }

    public class AddChatBotModel
    {
        [JsonProperty("projectName")]
        public string ProjectName { get; set; } = string.Empty;

        [JsonProperty("projectUrl")]
        public string ProjectUrl { get; set; } = string.Empty;

        [JsonProperty("modelName")]
        public AIChatModel ModelName { get; set; }

        [JsonProperty("encriptedApiKey")]
        public string EncriptedApiKey { get; set; } = string.Empty;

        [JsonProperty("encriptedDbConnectionString")]
        public string EncriptedDbConnectionString { get; set; } = string.Empty;

        [JsonProperty("backgroundColor")]
        public string BackgroundColor { get; set; } = "#FFFFFF";

        [JsonProperty("foregroundColor")]
        public string ForegroundColor { get; set; } = "#000000";

        [JsonProperty("textColor")]
        public string TextColor { get; set; } = "#333333";

        [JsonProperty("QuickActions")]
        public List<QuickActionModel> QuickActions { get; set; } = null!;

    }

    public class UpdateChatBotModel : AddChatBotModel
    {
        [JsonProperty("chatBotId")]
        public long ChatBotId { get; set; }

    }

    public class GetChatBotModel : UpdateChatBotModel
    {
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
