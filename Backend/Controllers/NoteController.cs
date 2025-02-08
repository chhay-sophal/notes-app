using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly NoteService _noteService;
        private readonly AuthService _authService;

        public NoteController(IConfiguration configuration, NoteService noteService, AuthService authService)
        {
            _configuration = configuration;
            _noteService = noteService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveNotes()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid token.");
            }

            var notes = await _noteService.GetActiveNotes(userId);
            return Ok(notes);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllNotes()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid token.");
            }

            var notes = await _noteService.GetAllNotes(userId);
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoteById(int id)
        {
            var note = await _noteService.GetNoteById(id);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] NoteDTO model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid note data" });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var note = new NoteItem
            {
                Title = model.Title,
                Content = model.Content,
                UserId = userId
            };

            await _noteService.CreateNote(note);

            return Ok(new { message = "Note created successfully", id = note.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] NoteDTO model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid note data" });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var note = await _noteService.GetNoteById(id);

            if (note == null)
            {
                return NotFound();
            }

            if (note.UserId != userId)
            {
                return Forbid();
            }

            note.Title = model.Title;
            note.Content = model.Content;

            await _noteService.UpdateNote(note);

            return Ok(new { message = "Note updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var note = await _noteService.GetNoteById(id);

            if (note == null)
            {
                return NotFound();
            }

            if (note.UserId != userId)
            {
                return Forbid();
            }

            await _noteService.DeleteNote(id);

            return Ok(new { message = "Note deleted successfully" });
        }
    }
}
