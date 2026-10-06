using System.ComponentModel.DataAnnotations;

namespace WellDoneGarageApi.Models
{
    public class Vehicle : BaseEntity
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public string VehicleNumber { get; set; } = string.Empty;
        [Required]
        public string Model { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation property
        public Customer? Customer { get; set; } = null!;
    }
}
