using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTO_s.Identity
{
    public class RegisterDto
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
    }
}
