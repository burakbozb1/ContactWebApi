using Bogus;
using ContactWeb.Core.DTOs;
using ContactWeb.Core.Entities;
using ContactWeb.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactWeb.Api.Controllers
{
    public class SetDataController : CustomBaseController
    {
        private readonly IContactService _contactService;
        private readonly IUserService _userService;
        private readonly IService<Location> _locationService;

        public SetDataController(IContactService contactService, IUserService userService, IService<Location> locationService)
        {
            _contactService = contactService;
            _userService = userService;
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int totalDataCount = 100000;
            int numberOfUsersContact = 1000; // totalDataCount % numberOfUsersContact must be 0 && (numberOfUsersContact < totalDataCount)
            int[] longitudes = new int[] { -180, 180 };
            int[] latitudes = new int[] { -90, 90 };
            var userFakeData = new Faker<User>()
                .RuleFor(x => x.Name, f => f.Name.FirstName())
                .RuleFor(x => x.MiddleName, f => f.Name.FirstName())
                .RuleFor(x => x.Password, f => "asd123")
                .RuleFor(x => x.Surname, f => f.Name.LastName())
                .RuleFor(x => x.Email, f => f.Person.Email)
                .RuleFor(x => x.CreatedDate, DateTime.Now)
                .RuleFor(x => x.UpdatedDate, DateTime.Now);

            var userData = userFakeData.Generate(100);
            await _userService.AddRangeAsync(userData);
            var users = userData.Select(x => x.Id).ToList();


            var LocationFakeData = new Faker<Location>()
                .RuleFor(x => x.Longitude, f => f.PickRandom(longitudes) * f.Random.Decimal())
                .RuleFor(x => x.Latitude, f => f.PickRandom(latitudes) * f.Random.Decimal());

            var locationData = LocationFakeData.Generate(totalDataCount);
            int locationCounter = 0;


            var ContactFakeData = new Faker<Contact>()
                .RuleFor(x => x.CreatedDate, DateTime.Now)
                .RuleFor(x => x.UpdatedDate, DateTime.Now)
                .RuleFor(x => x.Name, f => f.Name.FirstName())
                .RuleFor(x => x.MiddleName, f => f.Name.FirstName())
                .RuleFor(x => x.Surname, f => f.Name.LastName())
                .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(x => x.Note, f => String.Join(" ", f.Lorem.Words(5)))
                .RuleFor(x => x.Adress, f => f.Address.FullAddress())
                .RuleFor(x => x.Location, f => locationData[locationCounter++]);


            var contactData = ContactFakeData.Generate(totalDataCount);
            int lCounter = 0;

            foreach (var item in contactData)
            {
                item.Location = locationData[lCounter++];
            }

            int dataCounter = 0;
            foreach (var item in userData)
            {
                int remainDataCounter = contactData.Count - dataCounter;
                if (remainDataCounter >= numberOfUsersContact)
                {
                    item.Contacts = contactData.GetRange(dataCounter, numberOfUsersContact);
                }
                else if (remainDataCounter > 0)
                {
                    item.Contacts = contactData.GetRange(dataCounter, remainDataCounter);
                }
                else
                {
                    item.Contacts = contactData.GetRange(dataCounter, (-1 * remainDataCounter));
                }
                dataCounter += numberOfUsersContact;

            }

            foreach (var item in userData)
            {
                await _userService.UpdateAsync(item);
            }


            return CreateActionResult(CustomResponseDto<string>.Success(200, "Seed data is ready."));
        }
    }
}
