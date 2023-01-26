using AutoMapper;
using ContactWeb.Core.DTOs;
using ContactWeb.Core.Entities;
using ContactWeb.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Geolocation;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ContactWeb.Api.Controllers
{
    [Authorize]
    public class ContactController : CustomBaseController
    {
        private readonly IContactService _contactService;
        private readonly IUserService _userService;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IContactService contactService, IUserService userService, ILogger<ContactController> logger)
        {
            _contactService = contactService;
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// for testing.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contact = await _contactService.Where(x => x.Id == id)
                .Include(x => x.User)
                .Include(x => x.OldContacts)
                .Include(x => x.Location)
                .SingleOrDefaultAsync();
            if (contact != null)
            {
                _logger.LogCritical($"{id} contact listed.");
                return Ok(contact);
            }
            return NotFound();
        }

        [HttpGet("mycontacts")]
        public async Task<IActionResult> GetMyContacts()
        {
            var user = await _userService.GetByIdAsync(Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
            var result = await _contactService.GetMyContactsAsync(user);
            return CreateActionResult(CustomResponseDto<List<ContactDto>>.Success(201, result));
        }

        [HttpGet("getcontact/{id}")]
        public async Task<IActionResult> GetMyContacts(long id)
        {
            var user = await _userService.GetByIdAsync(Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
            var result = await _contactService.GetMyContactAsync(user,id);
            return CreateActionResult(CustomResponseDto<ContactWithOldContactsDto>.Success(201, result));
        }

        [HttpPost("addcontact")]
        public async Task<IActionResult> AddContact(AddContactDto newContact)
        {
            var user = await _userService.GetByIdAsync(Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
            var result = await _contactService.AddContactAsync(user, newContact);
            _logger.LogCritical($"{result.Id} contact added.");
            return CreateActionResult(CustomResponseDto<ContactDto>.Success(201, result));
        }

        [HttpPut("updatecontact")]
        public async Task<IActionResult> UpdateContact(ContactDto newContact)
        {
            var user = await _userService.GetByIdAsync(Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
            var result = await _contactService.UpdateContactAsync(user, newContact);
            if (result)
            {
                _logger.LogCritical($"{newContact.Id} contact updated.");
                return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
            }
            return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Contact failed to update."));
        }

        [HttpDelete("removecontact/{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var user = await _userService.GetByIdAsync(Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
            var result = await _contactService.RemoveContactAsync(user, id);
            if (result)
            {
                _logger.LogCritical($"{id} contact removed.");
                return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
            }
            return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Contact failed to remove."));
        }

        [HttpGet("searchbyname/{name}")]
        public async Task<IActionResult> SearchContactByName(string name)
        {
            var user = await _userService.GetByIdAsync(Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
            var result = await _contactService.SearchContactByNameAsync(user, name);
            return CreateActionResult(CustomResponseDto<List<ContactDto>>.Success(200, result));
        }

        [HttpPost("searchbydistance")]
        public async Task<IActionResult> SearchContactByDistance(SearchLocationDto location)
        {
            var user = await _userService.GetByIdAsync(Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
            var result = await _contactService.SearchContactByDistanceAsnyc(user, location);
            return CreateActionResult(CustomResponseDto<List<ContactDto>>.Success(200, result));
        }
    }
}
