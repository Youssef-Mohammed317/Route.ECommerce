using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.ProductDTOs
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string PictureUrl { get; set; } = null!;
        public string PictureName { get; set; } = null!;
        public Guid ProductBrandId { get; set; }
        public string ProductBrand { get; set; } = null!;
        public Guid ProductTypeId { get; set; }
        public string ProductType { get; set; } = null!;

    }
}
