using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactWeb.Core.DTOs
{
    public class ContactWithOldContactsDto:BaseDto
    {
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Note { get; set; }
        public string Adress { get; set; }
        public long UserId { get; set; }
        public LocationDto Location { get; set; }
        public List<OldContactDto> OldContacts { get; set; }
    }
}
