using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Contracts
{
    public interface ITokenServices
    {
        string CreateToken(string userId, string email, string userName, IEnumerable<string> roles);
    }
}
