using System.ComponentModel.DataAnnotations;

namespace WellDoneGarageApi.DTOs
{
    public class CustomerCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;
    }
}
