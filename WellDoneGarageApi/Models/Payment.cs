namespace WellDoneGarageApi.Models
{
    public class Payment : BaseEntity
    {
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }

        // Navigation property
        public Invoice Invoice { get; set; }
    }
}
