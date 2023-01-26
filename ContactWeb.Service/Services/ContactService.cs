using AutoMapper;
using ContactWeb.Core.DTOs;
using ContactWeb.Core.Entities;
using ContactWeb.Core.Repositories;
using ContactWeb.Core.Services;
using ContactWeb.Core.UnitOfWorks;
using ContactWeb.Service.Exceptions;
using Geolocation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ContactWeb.Service.Services
{
    public class ContactService : Service<Contact>, IContactService
    {
        private readonly IGenericRepository<Contact> _contactRepository;
        private readonly IMapper _mapper;

        //public ContactService(IGenericRepository<Contact> repository, IUnitOfWork unitOfWork, IMapper mapper): base(repository, unitOfWork)
        //{
        //    _repository = repository;
        //    _mapper = mapper;
        //}

        public ContactService(IUnitOfWork unitOfWork, IMapper mapper, IGenericRepository<Contact> contactRepository) : base(contactRepository, unitOfWork)
        {
            _mapper = mapper;
            _contactRepository = contactRepository;
        }

        public async Task<ContactDto> AddContactAsync(User user, AddContactDto newContact)
        {
            if (user != null)
            {
                var contact = _mapper.Map<Contact>(newContact);
                contact.CreatedDate = DateTime.Now;
                contact.UpdatedDate = DateTime.Now;
                contact.User = user;
                contact.UserId = user.Id;
                await _contactRepository.AddAsync(contact);
                await _unitOfWork.CommitAsync();
                var mappedContact = _mapper.Map<ContactDto>(contact);
                return mappedContact;
            }
            throw new NotFoundException("Add contact failed");

        }

        public async Task<ContactWithOldContactsDto> GetMyContactAsync(User user, long contactId)
        {
            var contacts = await _contactRepository.Where(x => x.UserId == user.Id && x.Id == contactId)
                .Include(x => x.OldContacts)
                .Include(x => x.Location)
                .AsNoTracking()
                .SingleOrDefaultAsync();
            var mappedContacts = _mapper.Map<ContactWithOldContactsDto>(contacts);
            return mappedContacts;
        }

        public async Task<List<ContactDto>> GetMyContactsAsync(User user)
        {
            var contacts = await _contactRepository.Where(x => x.UserId == user.Id)
                .Include(x => x.User)
                .Include(x => x.OldContacts)
                .Include(x => x.Location)
                .AsNoTracking()
                .ToListAsync();
            var mappedContacts = _mapper.Map<List<ContactDto>>(contacts);
            return mappedContacts;
        }

        public async Task<bool> RemoveContactAsync(User user, long contactId)
        {
            var contact = await _contactRepository.Where(x => (x.UserId == user.Id) && (x.Id == contactId)).SingleOrDefaultAsync();
            if (contact != null)
            {
                _contactRepository.Remove(contact);
                await _unitOfWork.CommitAsync();
                return true;
            }
            throw new NotFoundException("Remove contact failed");
        }

        public async Task<List<ContactDto>> SearchContactByDistanceAsnyc(User user, SearchLocationDto location)
        {
            var contacts = await _contactRepository.Where(x => x.UserId == user.Id && (x.Location != null)).Include(x => x.Location).AsNoTracking().ToListAsync();
            var result = contacts.Where(x => x.Location != null ? (DistanceCalculator(location, x.Location) < location.Distance) : false).ToList();
            var mappedContacts = _mapper.Map<List<ContactDto>>(result);
            return mappedContacts;
        }

        public async Task<List<ContactDto>> SearchContactByNameAsync(User user, string name)
        {
            var contacts = await _contactRepository.Where(x => x.UserId == user.Id && (x.Name.Contains(name) || x.MiddleName.Contains(name) || x.Surname.Contains(name))).AsNoTracking().ToListAsync();
            var mappedContacts = _mapper.Map<List<ContactDto>>(contacts);
            return mappedContacts;
        }

        public async Task<bool> UpdateContactAsync(User user, ContactDto newContact)
        {
            var contact = await _contactRepository.Where(x => x.Id == newContact.Id && x.UserId == user.Id)
                .Include(x => x.User)
                .Include(x => x.OldContacts)
                .Include(x => x.Location)
                .SingleOrDefaultAsync();

            //var contact = await _contactRepository.Where(x => x.Id == newContact.Id && x.UserId == user.Id)
            //.SingleOrDefaultAsync();
            if (contact != null)
            {
                List<OldContact> updatedDatas = new List<OldContact>();
                if (contact.Name != newContact.Name)
                {
                    updatedDatas.Add(new OldContact()
                    {
                        ChangedKey = "Name",
                        OldValue = contact.Name,
                        NewValue = newContact.Name,
                        Contact = contact,
                        ContactId = contact.Id,
                        CreatedDate = contact.CreatedDate,
                        UpdatedDate = DateTime.Now
                    });
                }

                if (contact.MiddleName != newContact.MiddleName)
                {
                    updatedDatas.Add(new OldContact()
                    {
                        ChangedKey = "MiddleName",
                        OldValue = contact.MiddleName,
                        NewValue = newContact.MiddleName,
                        Contact = contact,
                        ContactId = contact.Id,
                        CreatedDate = contact.CreatedDate,
                        UpdatedDate = DateTime.Now
                    });
                }

                if (contact.Surname != newContact.Surname)
                {
                    updatedDatas.Add(new OldContact()
                    {
                        ChangedKey = "Surname",
                        OldValue = contact.Surname,
                        NewValue = newContact.Surname,
                        Contact = contact,
                        ContactId = contact.Id,
                        CreatedDate = contact.CreatedDate,
                        UpdatedDate = DateTime.Now
                    });
                }

                if (contact.PhoneNumber != newContact.PhoneNumber)
                {
                    updatedDatas.Add(new OldContact()
                    {
                        ChangedKey = "PhoneNumber",
                        OldValue = contact.PhoneNumber,
                        NewValue = newContact.PhoneNumber,
                        Contact = contact,
                        ContactId = contact.Id,
                        CreatedDate = contact.CreatedDate,
                        UpdatedDate = DateTime.Now
                    });
                }

                if (contact.Note != newContact.Note)
                {
                    updatedDatas.Add(new OldContact()
                    {
                        ChangedKey = "Note",
                        OldValue = contact.Note,
                        NewValue = newContact.Note,
                        Contact = contact,
                        ContactId = contact.Id,
                        CreatedDate = contact.CreatedDate,
                        UpdatedDate = DateTime.Now
                    });
                }

                if (contact.Adress != newContact.Adress)
                {
                    updatedDatas.Add(new OldContact()
                    {
                        ChangedKey = "Adress",
                        OldValue = contact.Adress,
                        NewValue = newContact.Adress,
                        Contact = contact,
                        ContactId = contact.Id,
                        CreatedDate = contact.CreatedDate,
                        UpdatedDate = DateTime.Now
                    });
                }

                if (contact.Location != null && newContact.Location != null)
                {
                    if ((contact.Location.Latitude != newContact.Location.Latitude) || (contact.Location.Longitude != newContact.Location.Longitude))
                    {
                        updatedDatas.Add(new OldContact()
                        {
                            ChangedKey = "Latitude",
                            OldValue = $"Latitude :  {contact.Location.Latitude} - Longitude : {contact.Location.Longitude}",
                            NewValue = $"Longitude :  {contact.Location.Longitude} - Longitude : {contact.Location.Longitude}",
                            Contact = contact,
                            ContactId = contact.Id,
                            CreatedDate = contact.CreatedDate,
                            UpdatedDate = DateTime.Now
                        });
                    }
                }

                contact.UpdatedDate = DateTime.Now;
                if (contact.OldContacts == null)
                {
                    contact.OldContacts = new List<OldContact>();
                }
                contact.OldContacts.AddRange(updatedDatas);
                _contactRepository.Update(contact);
                await _unitOfWork.CommitAsync();
                return true;
            }
            throw new NotFoundException("Update contact failed");
        }

        private double DistanceCalculator(SearchLocationDto myLocation, Location destLocation)
        {
            var res = GeoCalculator.GetDistance((double)myLocation.Latitude, (double)myLocation.Longitude, (double)destLocation.Latitude, (double)destLocation.Longitude);
            return res;
        }
    }
}
