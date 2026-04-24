using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Maxishop.Web.Model
{
    [XmlRoot("Product")]
    public class Product
    {
        [Key]
        [XmlElement("id")]
        public int Id { get; set; }

        [Required]
        [XmlElement("category")]
        public string Category { get; set; }

        [Required]
        [XmlElement("name")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [XmlElement("price")]
        public decimal Price { get; set; }

        [Required]
        [XmlElement("manufacturedOn")]
        public DateOnly ManufacturedOn { get; set; }

        [XmlElement("inStock")]
        public bool InStock { get; set; }

        [XmlElement("note")]
        public string Note { get; set; }
    }
}
