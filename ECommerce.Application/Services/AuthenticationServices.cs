using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTO_s.Identity;

namespace ECommerce.Application.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly IIdentityServices identityServices;
        private readonly ITokenServices tokenServices;

        public AuthenticationServices(IIdentityServices identityServices,ITokenServices tokenServices)
        {
            this.identityServices = identityServices;
            this.tokenServices = tokenServices;
        }


        public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken ct = default)
        {
            var userResult = await identityServices.FindByEmailAsync(loginDto.Email, ct);

            if (userResult is null)
            {
                return Result<UserDto>.Fail(userResult.Errors);
            }

            var passwordCheck = await identityServices.CheckPasswordAsync(loginDto.Email,loginDto.Password,ct);

            if (passwordCheck is null)
            {
                return Result<UserDto>.Fail(Error.UnAuthorized("Invalid Email Or Password"));
            }

            var rolesResult = await identityServices.GetRolesAsync(userResult.data.Email);
            var token =  tokenServices.CreateToken(userResult.data.Id, userResult.data.Email, userResult.data.UserName, rolesResult.data);
            return Result<UserDto>.Ok(new UserDto()
            {
                Email = userResult.data.Email,
                DisplayName = userResult.data.DisplayName,
                Token = token
            });

        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            var result = await identityServices.CreateUserAsync(registerDto, ct);

            if(!result.IsSuccess || result.data is null)
            {
                return Result<UserDto>.Fail(result.Errors);
            }


            var rolesResult = await identityServices.GetRolesAsync(result.data.Email);
            var token = tokenServices.CreateToken(result.data.Id, result.data.Email, result.data.UserName, rolesResult.data);
            return Result<UserDto>.Ok(new UserDto()
            {
                Email = result.data.Email,
                DisplayName = result.data.DisplayName,
                Token = token
            });
        }



        public async Task<Result<bool>> CheckEmailAsync(string email, CancellationToken ct = default)
        {
            return await identityServices.EmailExistsAsync(email, ct);
        }  
        public async Task<Result<AddressDto>> GetUserAddressAsync(string email, CancellationToken ct = default)
        {
            var result = await identityServices.GetAddressByEmailAsync(email, ct);

            if (result is null)
            {
                return Result<AddressDto>.Fail(result.Errors);
            }

            return Result<AddressDto>.Ok(result.data);
        }

        public async Task<Result<AddressDto>> UpdateUserAddressAsync(AddressDto addressDto, string email, CancellationToken ct = default)
        {
            return await identityServices.UpdateAddressAsync(email,addressDto, ct);
        }

        public async Task<Result<UserDto>> GetCurrentUser(string email, CancellationToken ct = default)
        {
            var userResult = await identityServices.FindByEmailAsync(email, ct);
            if (!userResult.IsSuccess)
            {
                return Result<UserDto>.Fail(userResult.Errors);
            }

            var user = userResult.data;

            var rolesResult = await identityServices.GetRolesAsync(user.Email);

            if (!rolesResult.IsSuccess)
            {
                return Result<UserDto>.Fail(rolesResult.Errors);
            }

            var token = tokenServices.CreateToken(user.Id, user.Email, user.UserName, rolesResult.data);

            return Result<UserDto>.Ok(new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = token
            });
        }
    }
}
