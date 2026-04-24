using System.ComponentModel.DataAnnotations;

namespace Velora.Application.DTO.Category
{
    public class CategoryUpdateDto
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
