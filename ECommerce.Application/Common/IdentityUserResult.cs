using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Common
{
    public class IdentityUserResult
    {
        public IdentityUserResult(string id, string email, string userName, string displayName)
        {
            Id = id;
            Email = email;
            UserName = userName;
            DisplayName = displayName;
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }

    }
}
