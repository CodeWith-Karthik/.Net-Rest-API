using System.ComponentModel.DataAnnotations;

namespace Velora.Application.DTO.Brand
{
    public class BrandCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
