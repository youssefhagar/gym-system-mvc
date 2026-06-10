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
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(t =>
            {
                // Capacity between 1 and 25
                t.HasCheckConstraint(
                    "CK_Session_Capacity",
                    "[Capacity] >= 1 AND [Capacity] <= 25"
                );

                // EndDate must be after StartDate
                t.HasCheckConstraint(
                    "CK_Session_Dates",
                    "[EndDate] > [StartDate]"
                );
            });
        }
    }
}
