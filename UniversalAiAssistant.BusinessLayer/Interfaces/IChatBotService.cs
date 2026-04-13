using UniversalAiAssistant.Shared.Models;

namespace UniversalAiAssistant.Application.Interfaces
{
    public interface IChatBotService
    {
        Task<GetChatBotModel> CreateAsync(AddChatBotModel model, CancellationToken cancellationToken = default);
        Task<GetChatBotModel?> GetByIdAsync(long chatBotId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<GetChatBotModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<GetChatBotModel?> UpdateAsync(UpdateChatBotModel model, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(long chatBotId, CancellationToken cancellationToken = default);
        Task<TrainChatBotResponseModel> TrainAsync(long chatBotId, CancellationToken cancellationToken = default);
        Task<AskChatBotResponseModel> AskAsync(long chatBotId, AskChatBotModel model, CancellationToken cancellationToken = default);
    }
}
