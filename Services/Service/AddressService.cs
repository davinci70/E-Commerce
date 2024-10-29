using e_commerce.Data;
using e_commerce.Models.DTOs;
using e_commerce.Models.Entities;
using e_commerce.Services.IService;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Services.Service
{
    public class AddressService : IAddressService
    {
        private readonly AppDbContext _Context;

        public AddressService(AppDbContext Context)
        {
            _Context = Context;
        }
        public async Task<int> AddAsync(AddressDTO model)
        {
            if (string.IsNullOrWhiteSpace(model.Country))
            {
                throw new ArgumentException("Country can't be empty.");
            }
            
            if (string.IsNullOrWhiteSpace(model.City))
            {
                throw new ArgumentException("City can't be empty.");
            }
            
            if (string.IsNullOrWhiteSpace(model.State))
            {
                throw new ArgumentException("State can't be empty.");
            }
            
            if (string.IsNullOrWhiteSpace(model.Street))
            {
                throw new ArgumentException("Street can't be empty.");
            }

            if (model.PostalCode <= 0)
            {
                throw new ArgumentException("Postal code is not valid.");
            }

            var Address = new Address()
            {
                PostalCode = model.PostalCode,
                Country = model.Country,
                City = model.City,
                State = model.State,
                Street = model.Street,
            };

            await _Context.Addresses.AddAsync(Address);
            await _Context.SaveChangesAsync();

            return Address.AddressID;
        }

        public async Task<Address> GetByIdAsync(int ID)
        {
            if (ID <= 0)
            {
                throw new ValidationException("ID is not valid.");
            }

            var Address = await _Context.Addresses.FindAsync(ID);

            if (Address == null)
            {
                throw new InvalidOperationException("Address not found.");
            }

            return Address;
        }
    }
}
