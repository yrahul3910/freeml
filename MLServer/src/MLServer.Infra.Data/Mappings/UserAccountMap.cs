using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLServer.Domain.Models;

namespace MLServer.Infra.Data.Mappings
{
    public class UserAccountMap : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            // Primary Key.
            builder.HasKey(u => u.Id);

            // Properties.
            builder.Property(u => u.CreationDate)
                .IsRequired();

            builder.Property(u => u.UpdateDate);

            builder.Property(u => u.Name)
                .HasColumnType("varchar(20)")
                .HasMaxLength(20)
                .IsRequired();

            // Table & Column Mappings.
            builder.ToTable("UserAccounts");
        }
    }
}