using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactWeb.Core.DTOs
{
    public class SearchLocationDto
    {
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public double Distance { get; set; }
    }
}
