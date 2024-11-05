using e_commerce.Data;
using e_commerce.Helpers;
using e_commerce.Models.Auth;
using e_commerce.Models.DTOs;
using e_commerce.Models.DTOs.UserDTOs;
using e_commerce.Models.Entities;
using e_commerce.Models.Users;
using e_commerce.Services.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;

namespace e_commerce.Services.Service
{
    public class CustomerService : ICustomerService
    {
        private AppDbContext _Context = new AppDbContext();
        private readonly IAddressService _AddressService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _jwt;

        public CustomerService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, IAddressService addressService)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _AddressService = addressService;
        }

        public async Task<AuthModel> CustomerRegisterAsync(CustomerRegisterModel customerModel)
        {
            if (await _userManager.FindByEmailAsync(customerModel.userRegister.Email) is not null)
            {
                return new AuthModel { Message = "Email is already registered" };
            }

            if (await _userManager.FindByNameAsync(customerModel.userRegister.Username) is not null)
            {
                return new AuthModel { Message = "Username is already registered" };
            }

            var addressID = 0;

            try
            {
                addressID = await _AddressService.AddAsync(customerModel.userRegister.Address);
            }
            catch (ArgumentException ex)
            {
                return new AuthModel { Message = ex.Message };
            }

            var customer = new Customer()
            {
                AddressID = Convert.ToInt32(addressID),
                FirstName = customerModel.userRegister.FirstName,
                LastName = customerModel.userRegister.LastName,
                UserName = customerModel.userRegister.Username,
                Email = customerModel.userRegister.Email,
                PhoneNumber = customerModel.userRegister.PhoneNumber,
                DateCreated = DateTime.Now,
                IsActive = customerModel.userRegister.IsActive
            };

            var result = await _userManager.CreateAsync(customer, customerModel.userRegister.Password);

            if (!result.Succeeded)
            {
                // if the seller not added successfully, remove the address
                var address = await _Context.Addresses.FindAsync(addressID);
                if (address != null)
                {
                    _Context.Addresses.Remove(address);
                    await _Context.SaveChangesAsync();
                }

                var Errors = string.Empty;

                foreach (var error in result.Errors)
                {
                    Errors += $"{error.Description}, "[..^2];
                }

                return new AuthModel { Message = Errors };
            }

            await _userManager.AddToRoleAsync(customer, "Customer");
            var jwtSecurityToken = await JwtToken.CreateJwtToken(customer, _userManager, _jwt);

            return new AuthModel
            {
                Email = customer.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "Customer" },
                //Roles = _Context.Roles.Select(role => role.Name).ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = customer.UserName
            };
        }

        public async Task DeleteCustomerAsync(string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                throw new ValidationException("Id is not valid.");
            }

            var Customer = await _Context.Customers.FindAsync(ID);

            if (Customer == null)
            {
                throw new InvalidOperationException("Customer is not found.");
            }

            _Context.Customers.Remove(Customer);
            await _Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllAsync()
        {
            var Customers = await _Context.Customers
                .Include(x => x.Address)
                .Select(Customer => new CustomerDTO
                {
                    CustomerId = Customer.Id,
                    FirstName = Customer.FirstName,
                    LastName = Customer.LastName,
                    UserName = Customer.UserName,
                    Email = Customer.Email,
                    PhoneNumber = Customer.PhoneNumber,
                    Address = new AddressDTO
                    {
                        Country = Customer.Address.Country,
                        City = Customer.Address.City,
                        State = Customer.Address.State,
                        Street = Customer.Address.Street,
                        PostalCode = Customer.Address.PostalCode
                    },
                    IsActive = Customer.IsActive,
                    DateCreated = Customer.DateCreated
                }).ToListAsync();

            if (!Customers.Any())
            {
                throw new InvalidOperationException("No customers found.");
            }

            return Customers;
        }

        public async Task<CustomerDTO> GetByIdAsync(string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                throw new ValidationException("Id is not valid!");
            }

            var Customer = await _Context.Customers
                .Include(x => x.Address)
                .Where(x => x.Id == ID)
                .Select(Customer => new CustomerDTO
                {
                    CustomerId = Customer.Id,
                    FirstName = Customer.FirstName,
                    LastName = Customer.LastName,
                    UserName = Customer.UserName,
                    Email = Customer.Email,
                    PhoneNumber = Customer.PhoneNumber,
                    Address = new AddressDTO
                    {
                        Country = Customer.Address.Country,
                        City = Customer.Address.City,
                        State = Customer.Address.State,
                        Street = Customer.Address.Street,
                        PostalCode = Customer.Address.PostalCode
                    },
                    IsActive = Customer.IsActive,
                    DateCreated = Customer.DateCreated
                })
                .FirstOrDefaultAsync();

            if (Customer == null)
            {
                throw new InvalidOperationException("Customer not found.");
            }

            return Customer;
        }

        public async Task UpdateCustomerAsync(string ID, CustomerRegisterModel customerModel)
        {
            using (var transaction = await _Context.Database.BeginTransactionAsync())
            {
                try
                {
                    var customer = await _Context.Customers
                        .Include(x => x.Address)
                        .FirstOrDefaultAsync(x => x.Id == ID);

                    if (customer == null)
                    {
                        throw new InvalidOperationException("Admin not found.");
                    }

                    if (string.IsNullOrWhiteSpace(ID))
                    {
                        throw new ValidationException("Id is not valid!");
                    }

                    if (await _userManager.FindByEmailAsync(customerModel.userRegister.Email) is not null
                        && customer.Email != customerModel.userRegister.Email)
                    {
                        throw new ValidationException("Email is already registered");
                    }

                    if (await _userManager.FindByNameAsync(customerModel.userRegister.Username) is not null
                        && customer.UserName != customerModel.userRegister.Username)
                    {
                        throw new ValidationException("Username is already registered");
                    }

                    customer.FirstName = customerModel.userRegister.FirstName;
                    customer.LastName = customerModel.userRegister.LastName;
                    customer.UserName = customerModel.userRegister.Username;
                    customer.Email = customerModel.userRegister.Email;
                    customer.PhoneNumber = customerModel.userRegister.PhoneNumber;
                    customer.Address.Country = customerModel.userRegister.Address.Country;
                    customer.Address.City = customerModel.userRegister.Address.City;
                    customer.Address.State = customerModel.userRegister.Address.State;
                    customer.Address.Street = customerModel.userRegister.Address.Street;
                    customer.Address.PostalCode = customerModel.userRegister.Address.PostalCode;
                    customer.IsActive = customerModel.userRegister.IsActive;

                    await _Context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw new ValidationException("Failed to update customer");
                }
            }
        }
    }
}
