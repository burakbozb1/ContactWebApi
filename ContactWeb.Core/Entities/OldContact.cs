using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactWeb.Core.Entities
{
    public class OldContact : BaseEntity
    {
        public string ChangedKey { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public long ContactId { get; set; }
        public Contact Contact { get; set; }
    }
}
