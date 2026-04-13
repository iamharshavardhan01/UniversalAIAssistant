using DigiSoft.Common.Global;
using Newtonsoft.Json;

namespace UniversalAiAssistant.Shared.Models
{
    public class ThemeModel
    {
        [JsonProperty("backgroundColor")]
        public string BackgroundColor { get; set; } = "#FFFFFF";

        [JsonProperty("foregroundColor")]
        public string ForegroundColor { get; set; } = "#000000";

        [JsonProperty("textColor")]
        public string TextColor { get; set; } = "#333333";
    }

    public class AddChatBotModel
    {
        [JsonProperty("projectName")]
        public string ProjectName { get; set; } = string.Empty;

        [JsonProperty("projectUrl")]
        public string ProjectUrl { get; set; } = string.Empty;

        [JsonProperty("modelName")]
        public AIChatModel ModelName { get; set; }

        [JsonProperty("apiKey")]
        public string ApiKey { get; set; } = string.Empty;

        [JsonProperty("projectDbConnectionString")]
        public string ProjectDbConnectionString { get; set; } = string.Empty;

        [JsonProperty("theme")]
        public ThemeModel Theme { get; set; } = new();

        [JsonProperty("quickActions")]
        public List<AddQuickActionModel> QuickActions { get; set; } = new();

        [JsonProperty("crawlDepth")]
        public int CrawlDepth { get; set; } = 2;

        [JsonProperty("isActive")]
        public bool IsActive { get; set; } = true;
    }

    public class UpdateChatBotModel : AddChatBotModel
    {
        [JsonProperty("chatBotId")]
        public long ChatBotId { get; set; }
    }

    public class GetChatBotModel
    {
        [JsonProperty("chatBotId")]
        public long ChatBotId { get; set; }

        [JsonProperty("projectName")]
        public string ProjectName { get; set; } = string.Empty;

        [JsonProperty("projectUrl")]
        public string ProjectUrl { get; set; } = string.Empty;

        [JsonProperty("modelName")]
        public AIChatModel ModelName { get; set; }

        [JsonProperty("hasApiKey")]
        public bool HasApiKey { get; set; }

        [JsonProperty("hasProjectDbConnectionString")]
        public bool HasProjectDbConnectionString { get; set; }

        [JsonProperty("theme")]
        public ThemeModel Theme { get; set; } = new();

        [JsonProperty("quickActions")]
        public List<GetQuickActionModel> QuickActions { get; set; } = new();

        [JsonProperty("crawlDepth")]
        public int CrawlDepth { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("updatedDate")]
        public DateTime UpdatedDate { get; set; }

        [JsonProperty("lastTrainedAt")]
        public DateTime? LastTrainedAt { get; set; }
    }

    public class TrainChatBotResponseModel
    {
        [JsonProperty("chatBotId")]
        public long ChatBotId { get; set; }

        [JsonProperty("websitePagesCaptured")]
        public int WebsitePagesCaptured { get; set; }

        [JsonProperty("databaseArtifactsCaptured")]
        public int DatabaseArtifactsCaptured { get; set; }

        [JsonProperty("lastTrainedAt")]
        public DateTime LastTrainedAt { get; set; }
    }

    public class AskChatBotModel
    {
        [JsonProperty("sessionToken")]
        public string? SessionToken { get; set; }

        [JsonProperty("question")]
        public string Question { get; set; } = string.Empty;
    }

    public class AskChatBotResponseModel
    {
        [JsonProperty("chatBotId")]
        public long ChatBotId { get; set; }

        [JsonProperty("sessionToken")]
        public string SessionToken { get; set; } = string.Empty;

        [JsonProperty("answer")]
        public string Answer { get; set; } = string.Empty;

        [JsonProperty("theme")]
        public ThemeModel Theme { get; set; } = new();

        [JsonProperty("quickActions")]
        public List<GetQuickActionModel> QuickActions { get; set; } = new();
    }
}
