using System.ComponentModel.DataAnnotations;

namespace WellDoneGarageApi.DTOs
{
    public class VehicleOwnershipTransferDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int NewCustomerId { get; set; }
    }
}
