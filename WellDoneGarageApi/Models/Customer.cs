namespace WellDoneGarageApi.Models
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public ICollection<VehicleOwnership> VehicleOwnerships { get; set; } 
            = new List<VehicleOwnership>();
    }
}
