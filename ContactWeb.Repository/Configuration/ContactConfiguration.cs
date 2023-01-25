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
    internal class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Surname).IsRequired().HasMaxLength(50);
            builder.Property(x=> x.PhoneNumber).IsRequired().HasMaxLength(50);
            builder.HasOne(x => x.User).WithMany(x => x.Contacts).HasForeignKey(x => x.UserId);
            //builder.HasOne(x => x.Location).WithOne(x => x.Contact).HasForeignKey<Location>(x => x.ContactId);
        }
    }
}
