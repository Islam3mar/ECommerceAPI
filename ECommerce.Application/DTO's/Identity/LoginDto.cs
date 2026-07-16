using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTO_s.Identity
{
    public class LoginDto
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
