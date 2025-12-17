using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities.Base;

namespace Shared.Entities.Configurations
{
    public class BaseEntityConfiguration : IEntityTypeConfiguration<BaseEntity>
    {
        public void Configure(EntityTypeBuilder<BaseEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasConversion(
                    v => v, // when saving to database
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // when reading from database
                );

            builder.Property(e => e.CreatedByActorType)
                .HasColumnName("created_by_actor_type")
                .IsRequired();

            builder.Property(e => e.CreatedBy)
                .HasColumnName("created_by");

            builder.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp with time zone")
                .HasConversion(
                    v => v, // when saving to database
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null // when reading from database
                );

            builder.Property(e => e.UpdatedByActorType)
                .HasColumnName("updated_by_actor_type");

            builder.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by");

            builder.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at")
                .HasColumnType("timestamp with time zone")
                .HasConversion(
                    v => v, // when saving to database
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null // when reading from database
                );

            builder.Property(e => e.DeletedByActorType)
                .HasColumnName("deleted_by_actor_type");

            builder.Property(e => e.DeletedBy)
                .HasColumnName("deleted_by");
        }
    }
}

