using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.Common;
using ECommerce.Application.DTO_s.Products;

namespace ECommerce.Application.Contracts
{
    public interface IProductServices
    {
        Task<Result<IReadOnlyList<ProductDto>>> GetAllProductsAsync(CancellationToken ct = default);
        Task<Result<IReadOnlyList<BrandDto>>> GetAllProductBrandsAsync(CancellationToken ct = default);
        Task<Result<IReadOnlyList<TypeDto>>> GetAllProductTypesAsync(CancellationToken ct = default);

        Task<Result<ProductDto>> GetByIdAsync(int id, CancellationToken ct = default);


    }
}
