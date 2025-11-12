using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.ProductDTOs
{
    public class ProductTypeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
