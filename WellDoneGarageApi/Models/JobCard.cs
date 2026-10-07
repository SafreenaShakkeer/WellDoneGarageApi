using WellDoneGarageApi.Enums;

namespace WellDoneGarageApi.Models
{
    public class JobCard : BaseEntity
    {
        public int VehicleId { get; set; }
        public int CustomerId { get; set; }
        public string AssignedMechanic { get; set; } = string.Empty;
        public JobStatus Status { get; set; }
        public string ComplaintDescription { get; set; } = string.Empty;
        public DateTime? CompletedAt { get; set; }

        // Navigation properties
        public Vehicle Vehicle { get; set; } = null!;
        public Customer Customer { get; set; } = null!;
    }
}
