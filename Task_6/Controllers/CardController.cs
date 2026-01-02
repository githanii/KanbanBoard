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
        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            var all=await _service.GetCardsAsync();
            return Ok(all);
        }
        [HttpGet("list/{listId}")]
        public async Task<IActionResult> GetByListId(int listId)
        {
            var cards = await _service.GetCardsByListIdAsync(listId);
            return Ok(cards);
        }


        [HttpPost("{listId}")]
        public async Task<IActionResult> Create(int listId, CreateCardDto dto)
        {
            var card = await _service.CreateAsync(listId, dto.Title, dto.Description);

            if (card == null)
                return Forbid();

            return Ok(new CardDto
            {
                Id = card.Id,
                ListId = card.ListId,
                Title = card.Title,
                Description = card.Description,
                OrderIndex = card.OrderIndex,
            });
        }

        [HttpPut("{cardId:int}/update")]
        public async Task<IActionResult> Update(int cardId, UpdateCardDto dto)
        {
            var ok = await _service.UpdateAsync(cardId, dto.Title, dto.Description);
            return ok ? Ok() : Forbid();
        }

  
        
            [HttpDelete("{cardId:int}")]
            public async Task<IActionResult> Delete(int cardId)
            {
                var ok = await _service.SoftDeleteAsync(cardId);
                return ok ? Ok() : BadRequest();
            }
        




        [HttpPut("{cardId:int}/move")]
        public async Task<IActionResult> Move(int cardId, MoveCardDto dto)
        {
            var ok = await _service.MoveAsync(cardId, dto.TargetListId, dto.TargetOrderIndex);
            return ok ? Ok() : Forbid();
        }
    }
}
