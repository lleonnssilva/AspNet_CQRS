using AspNet_CQRS.Domain.Astractions;
using AspNet_CQRS.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using System.Text.Json;

namespace AspNet_CQRS.Application.Members.Queries
{
    public class GetMemberByIdQuery : IRequest<Member>
    {
        public int Id { get; set; }

        public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, Member>
        {
            private readonly IMemberDapperRepository _memberDapperRepository;
            private readonly IDistributedCache _redisCache;
            public GetMemberByIdQueryHandler(
                IMemberDapperRepository memberDapperRepository,
                IDistributedCache redisCache)
            {
                _memberDapperRepository = memberDapperRepository;
                _redisCache = redisCache;
            }
            public async Task<Member> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
            {
                var memberCache = await _redisCache.GetStringAsync($"{request.Id}");


                if (string.IsNullOrEmpty(memberCache))
                {
                    var memberFromDb = await _memberDapperRepository.GetMemberById(request.Id);

                    if (memberFromDb != null)
                    {
                        var memberJson = JsonSerializer.Serialize(memberFromDb);
                        await _redisCache.SetStringAsync($"{request.Id}", memberJson,
                            options: new DistributedCacheEntryOptions
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                            });
                    }

                    return memberFromDb;
                }


                var cachedMember = JsonSerializer.Deserialize<Member>(memberCache);

                if (cachedMember == null)
                {
                    var memberFromDb = await _memberDapperRepository.GetMemberById(request.Id);
                    return memberFromDb;
                }

                return cachedMember;
            }
        }
    }
}
