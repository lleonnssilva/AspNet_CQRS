using AspNet_CQRS.Domain.Entities;
using MediatR;

namespace AspNet_CQRS.Application.Members.Notifications
{
    public class MemberCreatedNotification : INotification
    {
        public Member Member { get; }
        public MemberCreatedNotification(Member member)
        {
            Member = member;
        }
    }
}
