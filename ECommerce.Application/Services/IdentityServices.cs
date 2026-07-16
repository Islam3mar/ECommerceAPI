using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTO_s.Identity;
using ECommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace ECommerce.Application.Services
{
    public class IdentityServices : IIdentityServices
    {
        private readonly UserManager<ApplicationUser> userManager;

        public IdentityServices(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<IdentityUserResult>> FindByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return Result<IdentityUserResult>.Fail(Error.Failure("User not found."));
            }
            else
            {
                return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.Email, user.UserName, user.DisplayName));
            }
        }
        public async Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return Result<bool>.Fail(Error.Failure("User not found"));
            }

            var isValid = await userManager.CheckPasswordAsync(user, password);

            return  Result<bool>.Ok(isValid);
        }

        public async Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
           var user = new ApplicationUser()
           {
               Email = registerDto.Email,
               UserName = registerDto.UserName,
               DisplayName = registerDto.DisplayName,
               PhoneNumber = registerDto.PhoneNumber
           };
            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new Error(e.Code, e.Description)).ToList();

                return Result<IdentityUserResult>.Fail(errors);
            }
                return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.Email, user.UserName, user.DisplayName));
            
        }

        public async Task<Result<IEnumerable<string>>> GetRolesAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return Result<IEnumerable<string>>.Fail(Error.Failure("User not found"));
            }

            var roles = await userManager.GetRolesAsync(user);
            return Result<IEnumerable<string>>.Ok(roles);
        }

        public async Task<Result<AddressDto>> GetAddressByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email, ct);

            if (user is null)
                return Result<AddressDto>.Fail(Error.NotFound("user not found"));
            
            if (user.Address is null)
                return Result<AddressDto>.Fail(Error.NotFound("address not found"));
            

            return Result<AddressDto>.Ok(new AddressDto
            {
                FirstName = user.Address.FirstName,
                LastName = user.Address.LastName,
                Street = user.Address.Street,
                City = user.Address.City,
                Country = user.Address.Country
            });
        }

        public async Task<Result<AddressDto>> UpdateAddressAsync(string email, AddressDto addressDto, CancellationToken ct = default)
        {
            var user = await userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email, ct);
            if (user is null)
            {
                return Result<AddressDto>.Fail(Error.NotFound("user not found"));
            }
            if (user.Address is null)
            {
                // Create New address if it doesn't exist
                user.Address = new Address()
               {
                   FirstName = addressDto.FirstName,
                   LastName = addressDto.LastName,
                   Street = addressDto.Street,
                   City = addressDto.City,
                   Country = addressDto.Country
               };
            }
            else
            {
                // Update existing address
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
                user.Address.Street = addressDto.Street;
                user.Address.City = addressDto.City;
                user.Address.Country = addressDto.Country;
            }
            
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return Result<AddressDto>.Fail(Error.Failure("Failed to update address"));


            return Result<AddressDto>.Ok(addressDto);
        }
        
        public async Task<Result<bool>> EmailExistsAsync(string email, CancellationToken ct = default)
        {
            return await userManager.FindByEmailAsync(email) is not null;
        }
    }
}
