using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.Common;
using ECommerce.Application.DTO_s.Identity;

namespace ECommerce.Application.Contracts
{
    public interface IAuthenticationServices
    {
        Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken ct = default);
        Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default);


        Task<Result<bool>> CheckEmailAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDto>> GetUserAddressAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDto>> UpdateUserAddressAsync(AddressDto addressDto, string email,CancellationToken ct = default);
        Task<Result<UserDto>> GetCurrentUser(string email, CancellationToken ct = default);

    }
}
