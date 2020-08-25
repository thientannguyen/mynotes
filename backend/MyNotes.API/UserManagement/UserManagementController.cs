using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyNotes.API.Controllers;
using MyNotes.Application.UserManagement.Commands;
using MyNotes.Application.UserManagement.Queries;
using MyNotes.Core.UserManagement.Entities;

namespace MyNotes.API.UserManagement
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public partial class UserManagementController : MyNotesController
    {
        private readonly IMediator _mediator;

        public UserManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateUser([FromBody] UserDto userDto)
        {
            var result = await _mediator.Send(new CreateUserCommand(userDto.Id, userDto.Name, 
                userDto.Notes.Select(MapToNote).ToList()));
            return result.Match(Ok, ServerError);
        }

        [HttpGet("GetUser")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetUser([Required] string userId)
        {
            var user = await _mediator.Send(new GetUserQuery(userId));
            if (user == null)
                return new NotFoundResult();

            return Ok(MapToUserDto(user));
        }

        private static Note MapToNote(NoteDto noteDto)
        {
            return new Note(noteDto.Id, noteDto.Title);
        }

        private static UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.id,
                Name = user.Name,
                Notes = user.Notes.Select(MapToNoteDto).ToArray()
            };
        }

        private static NoteDto MapToNoteDto(Note note)
        {
            return new NoteDto
            {
                Id = note.id,
                Title = note.Title
            };
        }
    }
}
