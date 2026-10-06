namespace WellDoneGarageApi.Models
{
    public class JobItem : BaseEntity
    {
        public int JobCardId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtTime { get; set; }

        // Navigation properties
        public JobCard JobCard { get; set; }
        public Item Item { get; set; }
    }
}
