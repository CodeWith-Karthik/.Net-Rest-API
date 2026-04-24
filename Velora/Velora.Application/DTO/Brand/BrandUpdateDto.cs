using System.ComponentModel.DataAnnotations;

namespace Velora.Application.DTO.Brand
{
    public class BrandUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
