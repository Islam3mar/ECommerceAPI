using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Common
{
    public class PaginatedResult<TEntity>
    {
        public PaginatedResult(IReadOnlyList<TEntity> data, int pageIndex, int pageSize, int count)
        {
            this.data = data;
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
            Count = count;
        }

        public IReadOnlyList<TEntity> data { get; set; } = [];
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int Count { get; set; }
       
    }
}
