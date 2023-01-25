using ContactWeb.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactWeb.Repository.Seeds
{
    internal class ContactSeed //: IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            throw new NotImplementedException();
            //builder.HasData(new Category()
            //{
            //    Id = 1,
            //    Name = "Kalemler"
            //},
            //    new Category()
            //    {
            //        Id = 2,
            //        Name = "Kitaplar"
            //    },
            //    new Category()
            //    {
            //        Id = 3,
            //        Name = "Defterler"
            //    }
            //);
        }
    }
}
