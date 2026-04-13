using Microsoft.AspNetCore.Mvc;
using UniversalAiAssistant.Application.Interfaces;
using UniversalAiAssistant.Shared.Models;

namespace UniversalAIAssistant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatBotsController : ControllerBase
    {
        private readonly IChatBotService _chatBotService;

        public ChatBotsController(IChatBotService chatBotService)
        {
            _chatBotService = chatBotService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GetChatBotModel>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _chatBotService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<GetChatBotModel>> GetById(long id, CancellationToken cancellationToken)
        {
            var result = await _chatBotService.GetByIdAsync(id, cancellationToken);
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<GetChatBotModel>> Create([FromBody] AddChatBotModel model, CancellationToken cancellationToken)
        {
            var result = await _chatBotService.CreateAsync(model, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.ChatBotId }, result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<GetChatBotModel>> Update(long id, [FromBody] UpdateChatBotModel model, CancellationToken cancellationToken)
        {
            if (id != model.ChatBotId)
            {
                return BadRequest("Route id and model chatBotId must match.");
            }

            var result = await _chatBotService.UpdateAsync(model, cancellationToken);
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
        {
            var deleted = await _chatBotService.DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{id:long}/train")]
        public async Task<ActionResult<TrainChatBotResponseModel>> Train(long id, CancellationToken cancellationToken)
        {
            var result = await _chatBotService.TrainAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost("{id:long}/ask")]
        public async Task<ActionResult<AskChatBotResponseModel>> Ask(long id, [FromBody] AskChatBotModel model, CancellationToken cancellationToken)
        {
            var result = await _chatBotService.AskAsync(id, model, cancellationToken);
            return Ok(result);
        }
    }
}
