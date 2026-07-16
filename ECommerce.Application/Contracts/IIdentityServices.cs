using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.Common;
using ECommerce.Application.DTO_s.Identity;

namespace ECommerce.Application.Contracts
{
    public interface IIdentityServices
    {
         // Login & Registration 
        Task<Result<IdentityUserResult>> FindByEmailAsync(string email , CancellationToken ct = default);
        Task<Result<bool>> CheckPasswordAsync (string email,string password , CancellationToken ct = default);
        Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDto registerDto ,CancellationToken ct = default);
        Task<Result<IEnumerable<string>>> GetRolesAsync(string email);


        Task<Result<AddressDto>> GetAddressByEmailAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDto>> UpdateAddressAsync(string email, AddressDto addressDto, CancellationToken ct = default);

        Task<Result<bool>> EmailExistsAsync(string email, CancellationToken ct = default);



    }
}
