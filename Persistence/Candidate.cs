using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence
{
    public class Candidate
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string PostalCode { get; set; }
    }

    public class CandidateConfig : IEntityTypeConfiguration<Candidate>
    {
        public void Configure(EntityTypeBuilder<Candidate> builder)
        {
            builder.ToTable(nameof(Candidate));

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).UseIdentityColumn();

            builder.Property(c => c.FirstName);

            builder.OwnsOne(c => c.Address,
                navigationBuilder =>
                {
                    navigationBuilder.Property(e => e.Street).HasColumnName(nameof(Address.Street));
                    navigationBuilder.Property(e => e.PostalCode).HasColumnName(nameof(Address.PostalCode));
                });
        }
    }
}
