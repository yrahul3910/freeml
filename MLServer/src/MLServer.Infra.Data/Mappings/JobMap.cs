using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLServer.Domain.Models;

namespace MLServer.Infra.Data.Mappings
{
    public class JobMap : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            // Primary Key.
            builder.HasKey(j => j.Id);

            // Properties.
            builder.Property(j => j.CreationDate)
                .IsRequired();

            builder.Property(j => j.UpdateDate);

            builder.Property(j => j.Name)
                .HasColumnType("varchar(100)")
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(j => j.Description)
                .HasColumnType("varchar")
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(j => j.Status)
                .IsRequired();

            // Table & Column Mappings.
            builder.ToTable("Jobs");
        }
    }
}