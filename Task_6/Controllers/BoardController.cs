using Application.Abstraction;
using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BoardController : ControllerBase
    {
        private readonly IUserContext _user;
        private readonly BoardRepoServices _services;

        public BoardController(BoardRepoServices services, IUserContext user )
        {
            _services = services;
            _user = user;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = _user.GetCurrentUserId();
            var boards = await _services.GetMyBoardsAsync(userId);
            return Ok(boards);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = _user.GetCurrentUserId();
            var board = await _services.GetMyBoardByIdAsync(id, userId);
            return board == null ? NotFound() : Ok(board);

        }
            [HttpPost]
        public async Task<IActionResult> Create(CreateBoardDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Board name is required");

            var board = new Board
            {
                Name = dto.Name.Trim(),
                OwnerId = _user.GetCurrentUserId(),
                CreatedAt = DateTime.UtcNow
            };

            var created = await _services.AddBoardAsync(board);
            return Ok(created);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _user.GetCurrentUserId();

            var board = await _services.GetMyBoardByIdAsync(id, userId);
            if (board == null) return NotFound();

            var deleted = await _services.DeleteBoardAsync(id);
            return deleted == 0 ? NotFound() : NoContent();
        }


    }
}
