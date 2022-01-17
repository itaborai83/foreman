using Foreman.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foreman.Persistence.Contexts.Contexts.Configurations
{
    public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
    {
        public void Configure(EntityTypeBuilder<WorkItem> builder)
        {
            builder.ToTable("WORK_ITEM");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.Data).IsRequired();
            builder.Property(p => p.Status).IsRequired();
            builder.Property(p => p.CreatedAt);
            builder.Property(p => p.AssignedAt);
            builder.Property(p => p.StartedAt);
            builder.Property(p => p.EndedAt);
            builder.HasCheckConstraint("WORK_ITEM_CK001", "[Status] in (1,2,3,4)"); // 
        }
    }
}



