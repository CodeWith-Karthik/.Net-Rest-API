using System.ComponentModel.DataAnnotations;


namespace Velora.Application.DTO.Category
{
    public class CategoryCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
