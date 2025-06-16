using AspNet_CQRS.Domain.Entities;

namespace AspNet_CQRS.Domain.Astractions
{
    public interface IMemberDapperRepository
    {
        Task<IEnumerable<Member>> GetMembers();
        Task<Member> GetMemberById(int id);
    }
}
