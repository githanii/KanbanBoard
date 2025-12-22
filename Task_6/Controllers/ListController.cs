using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ListsController : ControllerBase
    {
        private readonly ListService _service;

        public ListsController(ListService service)
        {
            _service = service;
        }

        [HttpPost("{boardId}")]
        public async Task<IActionResult> Create(int boardId, CreateListDto dto)
        {
            var list = await _service.CreateAsync(boardId, dto.Title);
            return list == null ? Forbid() : Ok(list);
        }

        [HttpPut("{listId}")]
        public async Task<IActionResult> Update(int listId, UpdateListDto dto)
        {
            var ok = await _service.UpdateAsync(listId, dto.Title, dto.OrderIndex);
            return ok ? Ok() : Forbid();
        }

        [HttpDelete("api/lists/{listId:int}")]
        public async Task<IActionResult> Delete(int listId)
        {
            var ok = await _service.DeleteAsync(listId);
            return ok ? NoContent() : Forbid();
        }
    }
}
