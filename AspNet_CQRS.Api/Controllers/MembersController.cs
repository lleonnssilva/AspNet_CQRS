using AspNet_CQRS.Application.Members.Commands;
using AspNet_CQRS.Application.Members.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspNet_CQRS.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MembersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetMembers()
        {
            var query = new GetMembersQuery();
            var members = await _mediator.Send(query);
            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMember(int id)
        {
            var query = new GetMemberByIdQuery { Id = id };
            var member = await _mediator.Send(query);

            return member != null ? Ok(member) : NotFound("Membro não encontrado.");
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberCommand command)
        {
            var createdMember = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetMember), new { id = createdMember.Id }, createdMember);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, UpdateMemberCommand command)
        {
            command.Id = id;
            var updatedMember = await _mediator.Send(command);

            return updatedMember != null ? Ok(updatedMember) : NotFound("Membro não encontrado.");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var command = new DeleteMemberCommand { Id = id };
            var deletedMember = await _mediator.Send(command);

            return deletedMember != null ? Ok(deletedMember) : NotFound("Membro não encontrado.");
        }
    }
}
