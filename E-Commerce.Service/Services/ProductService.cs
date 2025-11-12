using AutoMapper;
using AutoMapper.QueryableExtensions;
using E_Commerce.Domian.Entites.ProductModule;
using E_Commerce.Domian.Interfaces;
using E_Commerce.Service.Abstraction.Interfaces;
using E_Commerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service.Implementation.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.GetRepository<Product>().GetAllAsync();

            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }
        public async Task<IEnumerable<ProductBrandDTO>> GetAllProductBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand>().GetAllAsync();

            return _mapper.Map<IEnumerable<ProductBrandDTO>>(brands);
        }
        public async Task<IEnumerable<ProductTypeDTO>> GetAllProductTypesAsync()
        {
            var types = await _unitOfWork.GetRepository<ProductType>().GetAllAsync();

            return _mapper.Map<IEnumerable<ProductTypeDTO>>(types);
        }
        public async Task<ProductDTO> GetProductByIdAsync(Guid id)
        {
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(id);

            return _mapper.Map<ProductDTO>(product);
        }

    }
}
