using System.ComponentModel.DataAnnotations;
using WellDoneGarageApi.Enums;

namespace WellDoneGarageApi.DTOs
{
    public class JobCardCreateDto
    {
        [Range(1, int.MaxValue)]
        public int VehicleId { get; set; }
        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }
        [Required]
        public string AssignedMechanic { get; set; } = string.Empty;
        [Required]
        public string ComplaintDescription { get; set; } = string.Empty;
    }
}
