using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly CardService _service;

        public CardsController(CardService service)
        {
            _service = service;
        }

        [HttpPost("{listId}")]
        public async Task<IActionResult> Create(int listId, CreateCardDto dto)
        {
            var card = await _service.CreateAsync(listId, dto.Title, dto.Description);
            return card == null ? Forbid() : Ok(card);
        }

        [HttpPut("api/cards/{cardId:int}")]
        public async Task<IActionResult> Update(int cardId, UpdateCardDto dto)
        {
            var ok = await _service.UpdateAsync(cardId, dto.Title, dto.Description);
            return ok ? Ok() : Forbid();
        }

        [HttpDelete("{cardId:int}")]
        public async Task<IActionResult> Delete(int cardId)
        {
            var ok = await _service.SoftDeleteAsync(cardId);
            return ok ? NoContent() : Forbid();
        }

        [HttpPut("{cardId:int}/move")]
        public async Task<IActionResult> Move(int cardId, MoveCardDto dto)
        {
            var ok = await _service.MoveAsync(cardId, dto.TargetListId, dto.TargetOrderIndex);
            return ok ? Ok() : Forbid();
        }
    }
}
