using AspNet_CQRS.Domain.Astractions;
using AspNet_CQRS.Domain.Entities;
using AspNet_CQRS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AspNet_CQRS.Infrastructure.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        protected readonly AppDbContext _db;

        public MemberRepository(AppDbContext db)
        {
            this._db = db;
        }

        public async Task<Member> AddMember(Member member)
        {
            if (member == null) throw new ArgumentNullException("member");
            
            await _db.Members.AddAsync(member);
            return member;
        }

        public async Task<Member> DeleteMember(int id)
        {
            var member = await GetMemberById(id);
            if (member == null) throw new ArgumentNullException("member not found");

            _db.Members.Remove(member);
            return member;
        }

        public async Task<Member> GetMemberById(int id)
        {
            var member = await _db.Members.FindAsync(id);
            if (member == null) throw new ArgumentNullException("member not found");

            return member;
        }

        public async Task<IEnumerable<Member>> GetMembers()
        {
            var memberList = await _db.Members.ToListAsync();
            return memberList;
        }

        public void UpdateMember(Member member)
        {
            if (member == null) throw new ArgumentNullException("member");
            _db.Members.Update(member);
        }
    }
}
