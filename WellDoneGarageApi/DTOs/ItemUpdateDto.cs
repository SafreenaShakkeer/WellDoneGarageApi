using System.ComponentModel.DataAnnotations;

namespace WellDoneGarageApi.DTOs
{
    public class ItemUpdateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Unit price must be a non-negative value.")]
        public decimal UnitPrice { get; set; }
    }
}
