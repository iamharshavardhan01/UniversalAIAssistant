using DigiSoft.Common.Global;
using Newtonsoft.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UniversalAiAssistant.Application.Interfaces;
using UniversalAiAssistant.Domain.Entities;
using UniversalAiAssistant.Shared.Models;
using System.Text.RegularExpressions;
using MySqlConnector;
using System.Net.Http; 
namespace UniversalAIAssistant.Application.Services
{
    public class ChatBotService : IChatBotService
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpClientFactory _httpClientFactory;

        public ChatBotService(AppDbContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _dbContext = dbContext;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetChatBotModel> CreateAsync(AddChatBotModel model, CancellationToken cancellationToken = default)
        {
            Validate(model.ProjectName, model.ProjectUrl, model.Theme);

            var entity = new ChatBot
            {
                ProjectName = model.ProjectName.Trim(),
                ProjectUrl = model.ProjectUrl.Trim(),
                ModelName = model.ModelName,
                EncryptedApiKey = SecretEncryption.Encrypt(model.ApiKey),
                EncryptedDbConnectionString = SecretEncryption.Encrypt(model.ProjectDbConnectionString),
                BackgroundColor = model.Theme.BackgroundColor,
                ForegroundColor = model.Theme.ForegroundColor,
                TextColor = model.Theme.TextColor,
                CrawlDepth = model.CrawlDepth < 1 ? 1 : model.CrawlDepth,
                IsActive = model.IsActive,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            foreach (var quickAction in model.QuickActions.OrderBy(x => x.DisplayOrder))
            {
                entity.QuickActions.Add(new QuickAction
                {
                    ActionName = quickAction.ActionName.Trim(),
                    ActionUrl = quickAction.ActionUrl.Trim(),
                    DisplayOrder = quickAction.DisplayOrder
                });
            }

            _dbContext.ChatBots.Add(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Map(entity);
        }

        public async Task<bool> DeleteAsync(long chatBotId, CancellationToken cancellationToken = default)
        {
            var entity = await _dbContext.ChatBots
                .FirstOrDefaultAsync(x => x.Id == chatBotId, cancellationToken);

            if (entity is null)
            {
                return false;
            }

            _dbContext.ChatBots.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IReadOnlyList<GetChatBotModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _dbContext.ChatBots
                .Include(x => x.QuickActions)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync(cancellationToken);

            return entities.Select(Map).ToList();
        }

        public async Task<GetChatBotModel?> GetByIdAsync(long chatBotId, CancellationToken cancellationToken = default)
        {
            var entity = await _dbContext.ChatBots
                .Include(x => x.QuickActions)
                .FirstOrDefaultAsync(x => x.Id == chatBotId, cancellationToken);

            return entity is null ? null : Map(entity);
        }

        public async Task<GetChatBotModel?> UpdateAsync(UpdateChatBotModel model, CancellationToken cancellationToken = default)
        {
            Validate(model.ProjectName, model.ProjectUrl, model.Theme);

            var entity = await _dbContext.ChatBots
                .Include(x => x.QuickActions)
                .FirstOrDefaultAsync(x => x.Id == model.ChatBotId, cancellationToken);

            if (entity is null)
            {
                return null;
            }

            entity.ProjectName = model.ProjectName.Trim();
            entity.ProjectUrl = model.ProjectUrl.Trim();
            entity.ModelName = model.ModelName;
            entity.BackgroundColor = model.Theme.BackgroundColor;
            entity.ForegroundColor = model.Theme.ForegroundColor;
            entity.TextColor = model.Theme.TextColor;
            entity.CrawlDepth = model.CrawlDepth < 1 ? 1 : model.CrawlDepth;
            entity.IsActive = model.IsActive;
            entity.UpdatedDate = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(model.ApiKey))
            {
                entity.EncryptedApiKey = SecretEncryption.Encrypt(model.ApiKey);
            }

            if (!string.IsNullOrWhiteSpace(model.ProjectDbConnectionString))
            {
                entity.EncryptedDbConnectionString = SecretEncryption.Encrypt(model.ProjectDbConnectionString);
            }

            _dbContext.QuickActions.RemoveRange(entity.QuickActions);
            entity.QuickActions.Clear();
            foreach (var quickAction in model.QuickActions.OrderBy(x => x.DisplayOrder))
            {
                entity.QuickActions.Add(new QuickAction
                {
                    ActionName = quickAction.ActionName.Trim(),
                    ActionUrl = quickAction.ActionUrl.Trim(),
                    DisplayOrder = quickAction.DisplayOrder
                });
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Map(entity);
        }

        private static GetChatBotModel Map(ChatBot entity)
        {
            return new GetChatBotModel
            {
                ChatBotId = entity.Id,
                ProjectName = entity.ProjectName,
                ProjectUrl = entity.ProjectUrl,
                ModelName = entity.ModelName,
                HasApiKey = !string.IsNullOrWhiteSpace(entity.EncryptedApiKey),
                HasProjectDbConnectionString = !string.IsNullOrWhiteSpace(entity.EncryptedDbConnectionString),
                Theme = new ThemeModel
                {
                    BackgroundColor = entity.BackgroundColor,
                    ForegroundColor = entity.ForegroundColor,
                    TextColor = entity.TextColor
                },
                CrawlDepth = entity.CrawlDepth,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate,
                LastTrainedAt = entity.LastTrainedAt,
                QuickActions = entity.QuickActions
                    .OrderBy(x => x.DisplayOrder)
                    .Select(x => new GetQuickActionModel
                    {
                        QuickActionId = x.Id,
                        ActionName = x.ActionName,
                        ActionUrl = x.ActionUrl,
                        DisplayOrder = x.DisplayOrder
                    })
                    .ToList()
            };
        }

        private static void Validate(string projectName, string projectUrl, ThemeModel theme)
        {
            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new ArgumentException("Project name is required.", nameof(projectName));
            }

            if (string.IsNullOrWhiteSpace(projectUrl))
            {
                throw new ArgumentException("Project URL is required.", nameof(projectUrl));
            }

            if (!Uri.TryCreate(projectUrl, UriKind.Absolute, out _))
            {
                throw new ArgumentException("Project URL must be a valid absolute URL.", nameof(projectUrl));
            }

            if (string.IsNullOrWhiteSpace(theme.BackgroundColor) ||
                string.IsNullOrWhiteSpace(theme.ForegroundColor) ||
                string.IsNullOrWhiteSpace(theme.TextColor))
            {
                throw new ArgumentException("Theme colors are required.");
            }
        }

        public async Task<TrainChatBotResponseModel> TrainAsync(long chatBotId, CancellationToken cancellationToken = default)
        {
            var chatBot = await _dbContext.ChatBots
                .Include(x => x.CrawledPages)
                .FirstOrDefaultAsync(x => x.Id == chatBotId, cancellationToken)
                ?? throw new KeyNotFoundException($"Chat bot {chatBotId} was not found.");

            var websiteDocuments = await CrawlWebsiteAsync(chatBot.ProjectUrl, chatBot.CrawlDepth, cancellationToken);
            var dbArtifacts = await ReadDatabaseSchemaAsync(chatBot, cancellationToken);

            _dbContext.CrawledPages.RemoveRange(chatBot.CrawledPages);
            chatBot.CrawledPages.Clear();

            foreach (var doc in websiteDocuments)
            {
                chatBot.CrawledPages.Add(new CrawledPage
                {
                    Url = doc.Url,
                    Content = doc.Content,
                    SourceType = "website",
                    LastCrawled = DateTime.UtcNow
                });
            }

            foreach (var artifact in dbArtifacts)
            {
                chatBot.CrawledPages.Add(new CrawledPage
                {
                    Url = artifact.Key,
                    Content = artifact.Value,
                    SourceType = "database",
                    LastCrawled = DateTime.UtcNow
                });
            }

            chatBot.LastTrainedAt = DateTime.UtcNow;
            chatBot.UpdatedDate = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new TrainChatBotResponseModel
            {
                ChatBotId = chatBot.Id,
                WebsitePagesCaptured = websiteDocuments.Count,
                DatabaseArtifactsCaptured = dbArtifacts.Count,
                LastTrainedAt = chatBot.LastTrainedAt.Value
            };
        }

        public async Task<AskChatBotResponseModel> AskAsync(long chatBotId, AskChatBotModel model, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(model.Question))
            {
                throw new ArgumentException("Question is required.", nameof(model.Question));
            }

            var chatBot = await _dbContext.ChatBots
                .Include(x => x.QuickActions)
                .Include(x => x.CrawledPages)
                .FirstOrDefaultAsync(x => x.Id == chatBotId, cancellationToken)
                ?? throw new KeyNotFoundException($"Chat bot {chatBotId} was not found.");

            if (!chatBot.IsActive)
            {
                throw new InvalidOperationException("Chat bot is inactive.");
            }

            var session = await ResolveSessionAsync(chatBotId, model.SessionToken, cancellationToken);
            session.ChatMessages.Add(new ChatMessage
            {
                Role = "user",
                Content = model.Question.Trim(),
                Timestamp = DateTime.UtcNow
            });

            var contextText = BuildContext(chatBot.CrawledPages);
            var prompt = BuildPrompt(chatBot.ProjectName, contextText, model.Question.Trim());
            var answer = await GenerateAiAnswerAsync(chatBot, prompt, cancellationToken);

            session.ChatMessages.Add(new ChatMessage
            {
                Role = "assistant",
                Content = answer,
                Timestamp = DateTime.UtcNow
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new AskChatBotResponseModel
            {
                ChatBotId = chatBot.Id,
                SessionToken = session.SessionToken,
                Answer = answer,
                Theme = new ThemeModel
                {
                    BackgroundColor = chatBot.BackgroundColor,
                    ForegroundColor = chatBot.ForegroundColor,
                    TextColor = chatBot.TextColor
                },
                QuickActions = chatBot.QuickActions
                    .OrderBy(x => x.DisplayOrder)
                    .Select(x => new GetQuickActionModel
                    {
                        QuickActionId = x.Id,
                        ActionName = x.ActionName,
                        ActionUrl = x.ActionUrl,
                        DisplayOrder = x.DisplayOrder
                    })
                    .ToList()
            };
        }

        private async Task<ChatSession> ResolveSessionAsync(long chatBotId, string? sessionToken, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(sessionToken))
            {
                var existing = await _dbContext.ChatSessions
                    .Include(x => x.ChatMessages)
                    .FirstOrDefaultAsync(x => x.ChatBotId == chatBotId && x.SessionToken == sessionToken, cancellationToken);
                if (existing is not null)
                {
                    return existing;
                }
            }

            var session = new ChatSession
            {
                ChatBotId = chatBotId,
                StartedAt = DateTime.UtcNow,
                SessionToken = Guid.NewGuid().ToString("N")
            };

            _dbContext.ChatSessions.Add(session);
            return session;
        }

        private static string BuildContext(IEnumerable<CrawledPage> pages)
        {
            var selected = pages
                .OrderByDescending(x => x.LastCrawled)
                .Take(25)
                .Select(x => $"[{x.SourceType}] {x.Url}\n{x.Content}")
                .ToList();

            return selected.Count == 0
                ? "No trained knowledge exists yet."
                : string.Join("\n\n", selected);
        }

        private static string BuildPrompt(string projectName, string contextText, string question)
        {
            return $"""
You are a business assistant for project: {projectName}.
Use only the provided context to answer accurately.
If the answer is unavailable, say you need more data from admin training.

Context:
{contextText}

Question:
{question}
""";
        }

        private async Task<string> GenerateAiAnswerAsync(ChatBot chatBot, string prompt, CancellationToken cancellationToken)
        {
            var apiKey = SecureEncryptionProxy.Decrypt(chatBot.EncryptedApiKey);
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("AI API key is missing for this chat bot.");
            }

            var modelName = chatBot.ModelName.ToString().ToLowerInvariant();
            return modelName switch
            {
                "gemini" => await CallGeminiAsync(apiKey, prompt, cancellationToken),
                "claude" => await CallClaudeAsync(apiKey, prompt, cancellationToken),
                "deepseek" => await CallOpenAiStyleAsync("https://api.deepseek.com/v1/chat/completions", apiKey, modelName, prompt, cancellationToken),
                _ => await CallOpenAiStyleAsync("https://api.openai.com/v1/chat/completions", apiKey, "gpt-4o-mini", prompt, cancellationToken)
            };
        }

        private async Task<string> CallGeminiAsync(string apiKey, string prompt, CancellationToken cancellationToken)
        {
            var http = _httpClientFactory.CreateClient();
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";
            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            using var response = await http.PostAsync(url, CreateJson(payload), cancellationToken);
            response.EnsureSuccessStatusCode();
            var raw = await response.Content.ReadAsStringAsync(cancellationToken);
            dynamic data = JsonConvert.DeserializeObject(raw)!;
            return data?.candidates?[0]?.content?.parts?[0]?.text?.ToString() ?? "No answer generated.";
        }

        private async Task<string> CallClaudeAsync(string apiKey, string prompt, CancellationToken cancellationToken)
        {
            var http = _httpClientFactory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages");
            request.Headers.Add("x-api-key", apiKey);
            request.Headers.Add("anthropic-version", "2023-06-01");
            request.Content = CreateJson(new
            {
                model = "claude-3-5-sonnet-20241022",
                max_tokens = 700,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            });

            using var response = await http.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var raw = await response.Content.ReadAsStringAsync(cancellationToken);
            dynamic data = JsonConvert.DeserializeObject(raw)!;
            return data?.content?[0]?.text?.ToString() ?? "No answer generated.";
        }

        private async Task<string> CallOpenAiStyleAsync(string endpoint, string apiKey, string model, string prompt, CancellationToken cancellationToken)
        {
            var http = _httpClientFactory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Add("Authorization", $"Bearer {apiKey}");
            request.Content = CreateJson(new
            {
                model,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                temperature = 0.2
            });

            using var response = await http.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var raw = await response.Content.ReadAsStringAsync(cancellationToken);
            dynamic data = JsonConvert.DeserializeObject(raw)!;
            return data?.choices?[0]?.message?.content?.ToString() ?? "No answer generated.";
        }

        private static StringContent CreateJson(object payload)
        {
            return new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        }

        private async Task<List<(string Url, string Content)>> CrawlWebsiteAsync(string rootUrl, int crawlDepth, CancellationToken cancellationToken)
        {
            var http = _httpClientFactory.CreateClient();
            var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var queue = new Queue<(string Url, int Depth)>();
            var documents = new List<(string Url, string Content)>();
            var rootHost = new Uri(rootUrl).Host;

            queue.Enqueue((rootUrl, 0));

            while (queue.Count > 0)
            {
                var (url, depth) = queue.Dequeue();
                if (visited.Contains(url) || depth > crawlDepth)
                {
                    continue;
                }

                visited.Add(url);
                try
                {
                    using var response = await http.GetAsync(url, cancellationToken);
                    if (!response.IsSuccessStatusCode)
                    {
                        continue;
                    }

                    var html = await response.Content.ReadAsStringAsync(cancellationToken);
                    var text = SanitizeHtml(html);
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        documents.Add((url, text.Length > 4000 ? text[..4000] : text));
                    }

                    if (depth == crawlDepth)
                    {
                        continue;
                    }

                    foreach (var link in ExtractLinks(html, url))
                    {
                        if (link.Host.Equals(rootHost, StringComparison.OrdinalIgnoreCase))
                        {
                            queue.Enqueue((link.ToString(), depth + 1));
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }

            return documents;
        }

        private static IEnumerable<Uri> ExtractLinks(string html, string baseUrl)
        {
            var regex = new Regex("href\\s*=\\s*\"([^\"]+)\"", RegexOptions.IgnoreCase);
            foreach (Match match in regex.Matches(html))
            {
                var href = match.Groups[1].Value.Trim();
                if (string.IsNullOrWhiteSpace(href) || href.StartsWith("#") || href.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (Uri.TryCreate(new Uri(baseUrl), href, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                {
                    yield return uri;
                }
            }
        }

        private static string SanitizeHtml(string html)
        {
            var withoutScripts = Regex.Replace(html, "<script[\\s\\S]*?</script>", " ", RegexOptions.IgnoreCase);
            var withoutStyles = Regex.Replace(withoutScripts, "<style[\\s\\S]*?</style>", " ", RegexOptions.IgnoreCase);
            var plain = Regex.Replace(withoutStyles, "<[^>]+>", " ");
            plain = Regex.Replace(plain, "\\s+", " ").Trim();
            return plain;
        }

        private static async Task<Dictionary<string, string>> ReadDatabaseSchemaAsync(ChatBot chatBot, CancellationToken cancellationToken)
        {
            var results = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var connectionString = SecureEncryptionProxy.Decrypt(chatBot.EncryptedDbConnectionString);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return results;
            }

            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);

            const string tableSql = """
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = DATABASE()
ORDER BY TABLE_NAME;
""";

            using var tableCommand = new MySqlCommand(tableSql, connection);
            await using (var reader = await tableCommand.ExecuteReaderAsync(cancellationToken))
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    var tableName = reader.GetString(0);
                    results[$"db://table/{tableName}"] = $"Table: {tableName}";
                }
            }

            const string columnSql = """
SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = DATABASE()
ORDER BY TABLE_NAME, ORDINAL_POSITION;
""";

            using var columnCommand = new MySqlCommand(columnSql, connection);
            await using var columnReader = await columnCommand.ExecuteReaderAsync(cancellationToken);
            while (await columnReader.ReadAsync(cancellationToken))
            {
                var tableName = columnReader.GetString(0);
                var columnName = columnReader.GetString(1);
                var dataType = columnReader.GetString(2);
                var key = $"db://columns/{tableName}";
                var line = $"{columnName} ({dataType})";
                results[key] = results.TryGetValue(key, out var existing) ? $"{existing}, {line}" : line;
            }

            return results;
        }
    }

    internal static class SecretEncryption
    {
        public static string Encrypt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return SecureEncryptionProxy.Encrypt(value);
        }
    }

    internal sealed class SecureEncryptionProxy : EncryptionMethods
    {
        public static string Encrypt(string value) => Encipher(value);
        public static string Decrypt(string value) => Decipher(value);
    }
}
