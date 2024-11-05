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
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace e_commerce.Services.Service
{
    public class SellerService : ISellerService
    {
        private AppDbContext _Context = new AppDbContext();
        private readonly IAddressService _AddressService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _jwt;

        public SellerService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, IAddressService addressService)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _AddressService = addressService;
        }

        public async Task<AuthModel> SellerRegisterAsync(SellerRegisterModel sellerModel)
        {
            if (await _userManager.FindByEmailAsync(sellerModel.userRegister.Email) is not null)
            {
                return new AuthModel { Message = "Email is already registered" };
            }

            if (await _userManager.FindByNameAsync(sellerModel.userRegister.Username) is not null)
            {
                return new AuthModel { Message = "Username is already registered" };
            }

            var addressID = 0;

            try
            {
                addressID = await _AddressService.AddAsync(sellerModel.userRegister.Address);
            }
            catch (ArgumentException ex)
            {
                return new AuthModel { Message = ex.Message };
            }

            var seller = new Seller()
            {
                AddressID = Convert.ToInt32(addressID),
                FirstName = sellerModel.userRegister.FirstName,
                LastName = sellerModel.userRegister.LastName,
                UserName = sellerModel.userRegister.Username,
                Email = sellerModel.userRegister.Email,
                PhoneNumber = sellerModel.userRegister.PhoneNumber,
                DateCreated = DateTime.Now,
                IsActive = sellerModel.userRegister.IsActive
            };

            var result = await _userManager.CreateAsync(seller, sellerModel.userRegister.Password);

            if (!result.Succeeded)
            {
                // if the admin not added successfully, remove the address
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

            await _userManager.AddToRoleAsync(seller, "Seller");
            var jwtSecurityToken = await JwtToken.CreateJwtToken(seller, _userManager, _jwt);

            var store = new Store
            {
                StoreName = sellerModel.store.StoreName,
                Description = sellerModel.store.Description,
                SellerID = seller.Id
            };

            await _Context.Stores.AddAsync(store);
            await _Context.SaveChangesAsync();

            return new AuthModel
            {
                Email = seller.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "Seller" },
                //Roles = _Context.Roles.Select(role => role.Name).ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = seller.UserName
            };
        }
        public async Task UpdateSellerAsync(string ID, SellerRegisterModel sellerModel)
        {
            using (var transaction = await _Context.Database.BeginTransactionAsync())
            {
                try
                {
                    var seller = await _Context.Sellers
                        .Include(x => x.Address)
                        .Include(x => x.Store)
                        .FirstOrDefaultAsync(x => x.Id == ID);

                    if (seller == null)
                    {
                        throw new InvalidOperationException("Seller not found.");
                    }

                    if (string.IsNullOrWhiteSpace(ID))
                    {
                        throw new ValidationException("Id is not valid!");
                    }

                    if (await _userManager.FindByEmailAsync(sellerModel.userRegister.Email) is not null
                        && seller.Email != sellerModel.userRegister.Email)
                    {
                        throw new ValidationException("Email is already registered");
                    }

                    if (await _userManager.FindByNameAsync(sellerModel.userRegister.Username) is not null
                        && seller.UserName != sellerModel.userRegister.Username)
                    {
                        throw new ValidationException("Username is already registered");
                    }

                    seller.FirstName = sellerModel.userRegister.FirstName;
                    seller.LastName = sellerModel.userRegister.LastName;
                    seller.UserName = sellerModel.userRegister.Username;
                    seller.Email = sellerModel.userRegister.Email;
                    seller.PhoneNumber = sellerModel.userRegister.PhoneNumber;
                    seller.Address.Country = sellerModel.userRegister.Address.Country;
                    seller.Address.City = sellerModel.userRegister.Address.City;
                    seller.Address.State = sellerModel.userRegister.Address.State;
                    seller.Address.Street = sellerModel.userRegister.Address.Street;
                    seller.Address.PostalCode = sellerModel.userRegister.Address.PostalCode;
                    seller.IsActive = sellerModel.userRegister.IsActive;
                    seller.Store.StoreName = sellerModel.store.StoreName;
                    seller.Store.Description = sellerModel.store.Description;

                    await _Context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw new ValidationException("Failed to update admin");
                }
            }
        }
        public async Task<SellerDTO> GetByIdAsync(string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                throw new ValidationException("Id is not valid!");
            }

            var Seller = await _Context.Sellers
                .Include(x => x.Address)
                .Include(x => x.Store)
                .Where(x => x.Id == ID)
                .Select(Seller => new SellerDTO
                {
                    SellerId = Seller.Id,
                    FirstName = Seller.FirstName,
                    LastName = Seller.LastName,
                    UserName = Seller.UserName,
                    Email = Seller.Email,
                    PhoneNumber = Seller.PhoneNumber,
                    Address = new AddressDTO
                    {
                        Country = Seller.Address.Country,
                        City = Seller.Address.City,
                        State = Seller.Address.State,
                        Street = Seller.Address.Street,
                        PostalCode = Seller.Address.PostalCode
                    },
                    IsActive = Seller.IsActive,
                    DateCreated = Seller.DateCreated,
                    Store = new StoreDTO
                    {
                        StoreName = Seller.Store.StoreName,
                        Description = Seller.Store.Description
                    }
                })
                .FirstOrDefaultAsync();

            if (Seller == null)
            {
                throw new InvalidOperationException("Seller not found.");
            }

            return Seller;
        }
        public async Task<IEnumerable<SellerDTO>> GetAllAsync()
        {
            var Seller = await _Context.Sellers
                .Include(x => x.Address)
                .Include(x => x.Store)
                .Select(Seller => new SellerDTO
                {
                    SellerId = Seller.Id,
                    FirstName = Seller.FirstName,
                    LastName = Seller.LastName,
                    UserName = Seller.UserName,
                    Email = Seller.Email,
                    PhoneNumber = Seller.PhoneNumber,
                    Address = new AddressDTO
                    {
                        Country = Seller.Address.Country,
                        City = Seller.Address.City,
                        State = Seller.Address.State,
                        Street = Seller.Address.Street,
                        PostalCode = Seller.Address.PostalCode
                    },
                    IsActive = Seller.IsActive,
                    DateCreated = Seller.DateCreated,
                    Store = new StoreDTO
                    {
                        StoreName = Seller.Store.StoreName,
                        Description = Seller.Store.Description
                    }
                }).ToListAsync();

            if (!Seller.Any())
            {
                throw new InvalidOperationException("No sellers found.");
            }

            return Seller;
        }
        public async Task DeleteSellerAsync(string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                throw new ValidationException("Id is not valid.");
            }

            var Seller = await _Context.Sellers
                .FindAsync(ID);

            if (Seller == null)
            {
                throw new InvalidOperationException("Seller is not found.");
            }

            _Context.Sellers.Remove(Seller);
            await _Context.SaveChangesAsync();
        }
    }
}
