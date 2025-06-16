using AspNet_CQRS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNet_CQRS.Infrastructure.EntityConfiguration
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Gender).HasMaxLength(10).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(150).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();


            builder.HasData(
                new Member(1, "Leonardo", "Leite", "masculino", "leoguaruleo@gmail.com", true),
                new Member(2, "Denise", "Da Silveira", "feminino", "denisede@gmail.com", true),
                new Member(3, "Davi", "Lucca", "masculino", "daviluca@gmail.com", false)
            );
        }
    }
}
