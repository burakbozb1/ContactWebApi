using ContactWeb.Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactWeb.Service.Validation
{
    public class LocationDtoValidator:AbstractValidator<LocationDto>
    {
        public LocationDtoValidator()
        {
            RuleFor(x => (double)x.Longitude).ExclusiveBetween(-180.0, 180.0);
            RuleFor(x => (double)x.Latitude).ExclusiveBetween(-90.0, 900.0);
        }
    }
}
