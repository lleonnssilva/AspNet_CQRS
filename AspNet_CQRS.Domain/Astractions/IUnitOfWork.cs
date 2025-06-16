namespace AspNet_CQRS.Domain.Astractions
{
    public interface IUnitOfWork
    {
        IMemberRepository MemberRepository { get; }
        Task CommitAsync();
    }
}
