using ContactWeb.Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactWeb.Service.Validation
{
    public class SearchLocationDtoValidator: AbstractValidator<SearchLocationDto>
    {
        public SearchLocationDtoValidator()
        {
            RuleFor(x => (double)x.Longitude).ExclusiveBetween(-180.0, 180.0);
            RuleFor(x => (double)x.Latitude).ExclusiveBetween(-90.0, 90.0);
        }
    }
}
