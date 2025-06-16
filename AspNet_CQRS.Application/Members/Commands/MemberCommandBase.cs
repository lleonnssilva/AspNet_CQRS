using AspNet_CQRS.Domain.Entities;
using MediatR;

namespace AspNet_CQRS.Application.Members.Commands
{
    public abstract class MemberCommandBase : IRequest<Member>
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
