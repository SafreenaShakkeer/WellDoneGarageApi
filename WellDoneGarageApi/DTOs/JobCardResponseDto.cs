using System.ComponentModel.DataAnnotations;

namespace WellDoneGarageApi.DTOs
{
    public class JobCardResponseDto
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string VehicleNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string AssignedMechanic { get; set; } = string.Empty;
        public string ComplaintDescription { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? CompletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
