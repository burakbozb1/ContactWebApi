using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactWeb.Core.DTOs
{
    public class OldContactDto:BaseDto
    {
        public string ChangedKey { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
