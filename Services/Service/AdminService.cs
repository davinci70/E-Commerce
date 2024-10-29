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
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace e_commerce.Services.Service
{
    public class AdminService : IAdminService
    {
        private AppDbContext _Context = new AppDbContext();
        private readonly IAddressService _AddressService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _jwt;

        public AdminService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, IAddressService addressService)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _AddressService = addressService;
        }
        public async Task<AuthModel> AdminRegisterAsync(AdminRegisterModel adminModel)
        {
                    if (await _userManager.FindByEmailAsync(adminModel.userRegister.Email) is not null)
                    {
                        return new AuthModel { Message = "Email is already registered" };
                    }

                    if (await _userManager.FindByNameAsync(adminModel.userRegister.Username) is not null)
                    {
                        return new AuthModel { Message = "Username is already registered" };
                    }

                    var addressID = 0;

                    try
                    {
                        addressID = await _AddressService.AddAsync(adminModel.userRegister.Address);
                    }
                    catch (ArgumentException ex)
                    {
                        return new AuthModel { Message = ex.Message };
                    }

                    var Admin = new Admin()
                    {
                        AddressID = Convert.ToInt32(addressID),
                        FirstName = adminModel.userRegister.FirstName,
                        LastName = adminModel.userRegister.LastName,
                        UserName = adminModel.userRegister.Username,
                        Email = adminModel.userRegister.Email,
                        PhoneNumber = adminModel.userRegister.PhoneNumber,
                        DateCreated = DateTime.Now,
                        IsActive = adminModel.userRegister.IsActive,  
                        LastLogin = adminModel.LastLogin,
                        AdminNotes = adminModel.AdminNotes
                    };

                    var result = await _userManager.CreateAsync(Admin, adminModel.userRegister.Password);

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

                    await _userManager.AddToRoleAsync(Admin, "Admin");
                    var jwtSecurityToken = await JwtToken.CreateJwtToken(Admin, _userManager, _jwt);

                    return new AuthModel
                    {
                        Email = Admin.Email,
                        ExpiresOn = jwtSecurityToken.ValidTo,
                        IsAuthenticated = true,
                        Roles = new List<string> { "Admin"},
                        //Roles = _Context.Roles.Select(role => role.Name).ToList(),
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                        Username = Admin.UserName
                    };

            
        }
        public async Task UpdateAdminAsync(string ID, AdminRegisterModel adminModel)
        {
            using (var transaction = await _Context.Database.BeginTransactionAsync())
            {
                try
                {
                    var admin = await _Context.Admins
                        .Include(x => x.Address)
                        .FirstOrDefaultAsync(x => x.Id == ID);

                    if (admin == null)
                    {
                        throw new InvalidOperationException("Admin not found.");
                    }

                    if (string.IsNullOrWhiteSpace(ID))
                    {
                        throw new ValidationException("Id is not valid!");
                    }

                    if (await _userManager.FindByEmailAsync(adminModel.userRegister.Email) is not null
                        && admin.Email != adminModel.userRegister.Email)
                    {
                        throw new ValidationException("Email is already registered");
                    }

                    if (await _userManager.FindByNameAsync(adminModel.userRegister.Username) is not null
                        && admin.UserName != adminModel.userRegister.Username)
                    {
                        throw new ValidationException("Username is already registered");
                    }
                  
                    admin.FirstName = adminModel.userRegister.FirstName;
                    admin.LastName = adminModel.userRegister.LastName;
                    admin.UserName = adminModel.userRegister.Username;
                    admin.Email = adminModel.userRegister.Email;
                    admin.PhoneNumber = adminModel.userRegister.PhoneNumber;
                    admin.Address.Country = adminModel.userRegister.Address.Country;
                    admin.Address.City = adminModel.userRegister.Address.City;
                    admin.Address.State = adminModel.userRegister.Address.State;
                    admin.Address.Street = adminModel.userRegister.Address.Street;
                    admin.Address.PostalCode = adminModel.userRegister.Address.PostalCode;
                    admin.IsActive = adminModel.userRegister.IsActive;
                    admin.LastLogin = adminModel.LastLogin;
                    admin.AdminNotes = adminModel.AdminNotes;

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
        public async Task<AdminDTO> GetByIdAsync(string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                throw new ValidationException("Id is not valid!");
            }

            var Admin = await _Context.Admins
                .Include(x => x.Address)
                .Where(x => x.Id == ID)
                .Select(Admin => new AdminDTO
                {
                    AdminId = Admin.Id,
                    FirstName = Admin.FirstName,
                    LastName = Admin.LastName,
                    UserName = Admin.UserName,
                    Email = Admin.Email,
                    PhoneNumber = Admin.PhoneNumber,
                    Address = new AddressDTO
                    {
                        Country = Admin.Address.Country,
                        City = Admin.Address.City,
                        State = Admin.Address.State,
                        Street = Admin.Address.Street,
                        PostalCode = Admin.Address.PostalCode
                    },
                    IsActive = Admin.IsActive,
                    LastLogin = Admin.LastLogin,
                    AdminNotes = Admin.AdminNotes,
                    DateCreated = Admin.DateCreated
                })
                .FirstOrDefaultAsync();

            if (Admin == null)
            {
                throw new InvalidOperationException("Admin not found.");
            }

            return Admin;
        }
        public async Task<IEnumerable<AdminDTO>> GetAllAsync()
        {
            var Admins = await _Context.Admins
                .Include(x => x.Address)
                .Select(Admin => new AdminDTO
                {
                    AdminId = Admin.Id,
                    FirstName = Admin.FirstName,
                    LastName = Admin.LastName,
                    UserName = Admin.UserName,
                    Email = Admin.Email,
                    PhoneNumber = Admin.PhoneNumber,
                    Address = new AddressDTO
                    {
                        Country = Admin.Address.Country,
                        City = Admin.Address.City,
                        State = Admin.Address.State,
                        Street = Admin.Address.Street,
                        PostalCode = Admin.Address.PostalCode
                    },
                    IsActive = Admin.IsActive,
                    LastLogin = Admin.LastLogin,
                    AdminNotes = Admin.AdminNotes,
                    DateCreated = Admin.DateCreated
                }).ToListAsync();

            if (!Admins.Any())
            {
                throw new InvalidOperationException("No admins found.");
            }

            return Admins;
        }
        public async Task DeleteAdminAsync(string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                throw new ValidationException("Id is not valid.");
            }

            var Admin = await _Context.Admins
                .FindAsync(ID);

            if (Admin == null)
            {
                throw new InvalidOperationException("Admin is not found.");
            }

            _Context.Admins.Remove(Admin);
            await _Context.SaveChangesAsync();
        }
    }
}
