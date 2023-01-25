using ContactWeb.Core.DTOs;
using ContactWeb.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactWeb.Core.Services
{
    public interface IContactService : IService<Contact>
    {
        Task<List<ContactDto>> GetMyContactsAsync(User user);

        Task<ContactDto> AddContactAsync(User user, AddContactDto newContact);

        Task<bool> UpdateContactAsync(User user, ContactDto newContact);

        Task<bool> RemoveContactAsync(User user, long contactId);

        Task<List<ContactDto>> SearchContactByNameAsync(User user, string name);

        Task<List<ContactDto>> SearchContactByDistanceAsnyc(User user, SearchLocationDto location);
    }
}
