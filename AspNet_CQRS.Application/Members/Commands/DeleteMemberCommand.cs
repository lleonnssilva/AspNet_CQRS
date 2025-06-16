using AspNet_CQRS.Domain.Astractions;
using AspNet_CQRS.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace AspNet_CQRS.Application.Members.Commands
{
    public sealed class DeleteMemberCommand : IRequest<Member>
    {
        public int Id { get; set; }

        public class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand, Member>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IDistributedCache _redisCache;
            public DeleteMemberCommandHandler(IUnitOfWork unitOfWork, IDistributedCache redisCache)
            {
                _unitOfWork = unitOfWork;
                _redisCache = redisCache;
            }
            public async Task<Member> Handle(DeleteMemberCommand request,
                         CancellationToken cancellationToken)
            {
                var deletedMember = await _unitOfWork.MemberRepository.DeleteMember(request.Id);

                if (deletedMember is null)
                    throw new InvalidOperationException("Member not found");
                await _unitOfWork.CommitAsync();

                var memberCache = await _redisCache.GetStringAsync($"{request.Id}");
                if(memberCache is not null)
                    await _redisCache.RemoveAsync($"{request.Id}");
                return deletedMember;
            }
        }
    }
}
