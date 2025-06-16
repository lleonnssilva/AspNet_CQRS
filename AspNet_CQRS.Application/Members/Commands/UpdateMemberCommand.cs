using AspNet_CQRS.Domain.Astractions;
using AspNet_CQRS.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using System.Text.Json;

namespace AspNet_CQRS.Application.Members.Commands
{
    public sealed class UpdateMemberCommand : MemberCommandBase
    {
        public int Id { get; set; }
        public class UpdateMemberCommandHandler :
                     IRequestHandler<UpdateMemberCommand, Member>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IDistributedCache _redisCache;
            public UpdateMemberCommandHandler(IUnitOfWork unitOfWork,IDistributedCache redisCache)
            {
                _unitOfWork = unitOfWork;
                _redisCache = redisCache;
            }
            public async Task<Member> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
            {
                var existingMember = await _unitOfWork.MemberRepository.GetMemberById(request.Id);

                if (existingMember is null)
                    throw new InvalidOperationException("Member not found");

                existingMember.Update(request.FirstName, request.LastName, request.Gender, request.Email, request.IsActive);
                _unitOfWork.MemberRepository.UpdateMember(existingMember);
                await _unitOfWork.CommitAsync();

                var memberCache = await _redisCache.GetStringAsync($"{request.Id}");
                if (memberCache is not null)
                    await _redisCache.RemoveAsync($"{request.Id}");

                return existingMember;
            }
        }
    }
}
