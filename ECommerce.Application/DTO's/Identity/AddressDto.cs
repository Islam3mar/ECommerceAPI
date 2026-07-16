using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTO_s.Identity
{
    public class AddressDto
    {
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
    }
}
