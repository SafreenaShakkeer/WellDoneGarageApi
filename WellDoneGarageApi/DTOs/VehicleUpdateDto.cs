using System.ComponentModel.DataAnnotations;

namespace WellDoneGarageApi.DTOs
{
    public class VehicleUpdateDto
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public string VehicleNumber { get; set; } = string.Empty;
        [Required]
        public string Model { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
