using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTO_s.Products;
using ECommerce.Application.Params;
using ECommerce.Application.Specifications;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Application.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<Result<PaginatedResult<ProductDto>>> GetAllProductsAsync(ProductQueryParams queryParams, CancellationToken ct = default)
        {
            var specification = new ProductSpecifications(queryParams);

            var products = await unitOfWork.GetRepository<Product, int>()
                                .GetAllWithSpecificationsAsync(specification, ct);

            var mappedProducts = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products);

            //-----------------------------------------------------------------------------------------

            var countSpecification = new ProductCountSpecefication(queryParams);

            var totalCount = await unitOfWork.GetRepository<Product, int>()
                                .GetProductCountWithSpecificationsAsync(countSpecification, ct);

            return Result<PaginatedResult<ProductDto>>.Ok(new PaginatedResult<ProductDto>
                (mappedProducts, queryParams.PageIndex, products.Count, totalCount));
        }

        public async Task<Result<IReadOnlyList<BrandDto>>> GetAllProductBrandsAsync(CancellationToken ct = default)
        {
            var brands = await unitOfWork.GetRepository<ProductsBrand, int>().GetAllAsync(ct);

            var mappedBrands = mapper.Map<IReadOnlyList<ProductsBrand>, IReadOnlyList<BrandDto>>(brands);

            return Result<IReadOnlyList<BrandDto>>.Ok(mappedBrands);
        }
        public async Task<Result<IReadOnlyList<TypeDto>>> GetAllProductTypesAsync(CancellationToken ct = default)
        {
            var types = await unitOfWork.GetRepository<ProductsType, int>().GetAllAsync(ct);

            var mappedTypes = mapper.Map<IReadOnlyList<ProductsType>, IReadOnlyList<TypeDto>>(types);

            return Result<IReadOnlyList<TypeDto>>.Ok(mappedTypes);


        }

        public async Task<Result<ProductDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var specification = new ProductSpecifications(id);

            var product = await unitOfWork.GetRepository<Product,int>().GetByIdWithSpecificationsAsync(specification, ct);

            if (product is null)
                return Result<ProductDto>.Fail(Error.NotFound("Product Is Not Found", $"Product With ID : {id} Is Not Found"));


            var mappedProduct = mapper.Map<Product, ProductDto>(product);

            return mappedProduct;
        }
    }
}
