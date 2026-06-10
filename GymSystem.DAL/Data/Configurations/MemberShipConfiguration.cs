using GymSystem.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.Configurations
{
    internal class MemberShipConfiguration : IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreatedAt).HasColumnName("StartDate").HasDefaultValueSql("GETDATE()");

        }
    }
}
