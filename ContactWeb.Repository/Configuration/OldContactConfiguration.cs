using ContactWeb.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactWeb.Repository.Configuration
{
    internal class OldContactConfiguration : IEntityTypeConfiguration<OldContact>
    {
        public void Configure(EntityTypeBuilder<OldContact> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasOne(x => x.Contact).WithMany(x => x.OldContacts).HasForeignKey(x => x.ContactId);
        }
    }
}
