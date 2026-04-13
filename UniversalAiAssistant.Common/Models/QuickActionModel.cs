using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalAiAssistant.Shared.Models
{
    public class AddQuickActionModel
    {
        [JsonProperty("actionName")]
        public string ActionName { get; set; } = string.Empty;

        [JsonProperty("actionUrl")]
        public string ActionUrl { get; set; } = string.Empty;

        [JsonProperty("displayOrder")]
        public int DisplayOrder { get; set; }
    }

    public class UpdateQuickActionModel : AddQuickActionModel
    {
        [JsonProperty("quickActionId")]
        public long QuickActionId { get; set; }
    }

    public class GetQuickActionModel : UpdateQuickActionModel
    {

    }
}
