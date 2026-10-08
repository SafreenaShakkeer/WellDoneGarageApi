namespace WellDoneGarageApi.Models
{
    public class VehicleOwnership : BaseEntity
    {
        public int VehicleId { get; set; }
        public int CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        // Navigation properties
        public Vehicle? Vehicle { get; set; }
        public Customer? Customer { get; set; }
    }
}
