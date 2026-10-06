namespace WellDoneGarageApi.Models
{
    public class Invoice : BaseEntity
    {
        public int JobCardId { get; set; }
        public int CustomerId { get; set; }
        public decimal LabourCost { get; set; }
        public decimal PartsCost { get; set; }
        public decimal TotalAmount { get; set; }

        // Navigation properties
        public JobCard JobCard { get; set; }
        public Customer Customer { get; set; }
    }
}
