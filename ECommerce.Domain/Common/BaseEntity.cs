using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Common
{
    public class BaseEntity<TKey>
    {
        public TKey Id { get; set; } = default!; // Not default must have value
    }
}
