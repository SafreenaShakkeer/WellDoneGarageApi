using System.ComponentModel.DataAnnotations;
using WellDoneGarageApi.Enums;

namespace WellDoneGarageApi.DTOs
{
    public class JobCardUpdateDto
    {
        [Required]
        public string AssignedMechanic { get; set; } = string.Empty;
        [Required]
        public string ComplaintDescription { get; set; } = string.Empty;
        public JobStatus Status { get; set; }
    }
}
