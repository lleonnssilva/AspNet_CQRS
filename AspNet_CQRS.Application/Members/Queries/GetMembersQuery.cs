using AspNet_CQRS.Domain.Astractions;
using AspNet_CQRS.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace AspNet_CQRS.Application.Members.Queries
{
    public class GetMembersQuery : IRequest<IEnumerable<Member>>
    {
        public class GetMembersQueryHandler : IRequestHandler<GetMembersQuery, IEnumerable<Member>>
        {
            private readonly IMemberDapperRepository _memberDapperRepository;
            private readonly IDistributedCache _redidCache;
            public GetMembersQueryHandler(IMemberDapperRepository memberDapperRepository, IDistributedCache redidCache)
            {
                _memberDapperRepository = memberDapperRepository;

            }
            public async Task<IEnumerable<Member>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
            {
                var members = await _memberDapperRepository.GetMembers();
                return members;
            }
        }
    }
}
