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
        private readonly IImageService imageService;

        public ProductServices(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.imageService = imageService;
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



        public async Task<Result<ProductDto>> CreateAsync(CreateProductDto createDto, CancellationToken ct = default)
        {
            // 1. Validate Brand & Type actually exist before touching the file system / DB
            var brand = await unitOfWork.GetRepository<ProductsBrand, int>().GetByIdAsync(createDto.BrandId, ct);

            if (brand is null)
                return Result<ProductDto>.Fail(Error.NotFound("Brand.NotFound", $"Brand With ID : {createDto.BrandId} Is Not Found"));

            var type = await unitOfWork.GetRepository<ProductsType, int>().GetByIdAsync(createDto.TypeId, ct);

            if (type is null)
                return Result<ProductDto>.Fail(Error.NotFound("Type.NotFound", $"Type With ID : {createDto.TypeId} Is Not Found"));

            //------------------------------------------------------------------------------------------------

            // 2. Save the image to disk first — we need the resulting relative path before creating the entity
            var pictureUrl = await imageService.SaveImageAsync(createDto.Image, "Products", ct);

            var product = new Product
            {
                Name = createDto.Name,
                Description = createDto.Description,
                Price = createDto.Price,
                BrandId = createDto.BrandId,
                TypeId = createDto.TypeId,
                PictureUrl = pictureUrl
            };

            unitOfWork.GetRepository<Product, int>().Add(product);

            var result = await unitOfWork.SaveChangesAsync(ct);

            // 3. Roll back the saved file if the DB insert somehow failed — avoid orphan images on disk
            if (result <= 0)
            {
                imageService.DeleteImage(pictureUrl);
                return Result<ProductDto>.Fail(Error.Failure("Product.CreateFailed", "Product Could Not Be Created"));
            }

            // Brand/Type were loaded above without navigation include (GetByIdAsync uses FindAsync),
            // but we already have the full objects in hand, so attach them for the mapper (BrandName/TypeName).
            product.Brand = brand;
            product.Type = type;

            return Result<ProductDto>.Ok(mapper.Map<ProductDto>(product));
        }

        public async Task<Result<ProductDto>> UpdateAsync(int id, UpdateProductDto updateDto, CancellationToken ct = default)
        {
            // 1. Load the product WITH its Brand/Type (needed for the returned ProductDto mapping,
            // and so unsent Brand/Type stay correct in the response)
            var specification = new ProductSpecifications(id);

            var product = await unitOfWork.GetRepository<Product, int>().GetByIdWithSpecificationsAsync(specification, ct);

            if (product is null)
                return Result<ProductDto>.Fail(Error.NotFound("Product.NotFound", $"Product With ID : {id} Is Not Found"));

            //------------------------------------------------------------------------------------------------

           

           
            //------------------------------------------------------------------------------------------------

            // 4. Only touch the image if a new one was actually uploaded
            var oldPictureUrl = product.PictureUrl;
            string? newPictureUrl = null;

            if (updateDto.Image is not null)
            {
                newPictureUrl = await imageService.SaveImageAsync(updateDto.Image, "Products", ct);
                product.PictureUrl = newPictureUrl;
            }

            // 5. Only overwrite scalar fields that were actually sent — everything else keeps its original value
            if (!string.IsNullOrWhiteSpace(updateDto.Name) && !(updateDto.Name == "string"))
                product.Name = updateDto.Name;

            if (!string.IsNullOrWhiteSpace(updateDto.Description) && !(updateDto.Description == "string"))
                product.Description = updateDto.Description;

            if (updateDto.Price is not null && !(updateDto.Price == 0))
                product.Price = updateDto.Price.Value;

            unitOfWork.GetRepository<Product, int>().Update(product);

            var result = await unitOfWork.SaveChangesAsync(ct);

            if (result <= 0)
            {
                // Roll back the newly uploaded image if the save failed — keep the old one intact
                if (newPictureUrl is not null)
                    imageService.DeleteImage(newPictureUrl);

                return Result<ProductDto>.Fail(Error.Failure("Product.UpdateFailed", "Product Could Not Be Updated"));
            }

            // Save succeeded and a new image replaced the old one => now it's safe to delete the old file
            if (newPictureUrl is not null)
                imageService.DeleteImage(oldPictureUrl);

            return Result<ProductDto>.Ok(mapper.Map<ProductDto>(product));
        }

        public async Task<Result<bool>> DeleteAsync(int id, CancellationToken ct = default)
        {
            var product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync(id, ct);

            if (product is null)
                return Result<bool>.Fail(Error.NotFound("Product.NotFound", $"Product With ID : {id} Is Not Found"));

            unitOfWork.GetRepository<Product, int>().Delete(product);

            var result = await unitOfWork.SaveChangesAsync(ct);

            if (result <= 0)
                return Result<bool>.Fail(Error.Failure("Product.DeleteFailed", "Product Could Not Be Deleted"));

            // Only delete the physical file after the DB row is confirmed gone
            imageService.DeleteImage(product.PictureUrl);

            return Result<bool>.Ok(true);
        }
    }
}

