using GymSystem.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.Configurations
{
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {

            // Name
            builder.Property(u => u.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            // Email
            builder.Property(u => u.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            // Phone
            builder.Property(u => u.Phone)
                .HasMaxLength(11)
                .IsRequired();

            builder.HasIndex(u => u.Phone)
                .IsUnique();


            // Email Check Constraint
            builder.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_GymUser_Email",
                    "[Email] LIKE '%_@_%._%'"
                );

                t.HasCheckConstraint(
                    "CK_GymUser_Phone",
                    "Phone LIKE '010%' OR Phone LIKE '011%' OR Phone LIKE '012%' OR Phone LIKE '015%'"
                );
            });


            builder.OwnsOne(u => u.Address, address =>
            {
                address.Property(a => a.Street).HasColumnType("varchar").HasMaxLength(30);
                address.Property(a => a.City).HasColumnType("varchar").HasMaxLength(30);
            });

        }
    }
}
