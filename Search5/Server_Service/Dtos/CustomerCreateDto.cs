using System.ComponentModel.DataAnnotations;

namespace Server_Service.Dtos
{
    public class CustomerCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Surname { get; set; } = null!;

        [Required]
        public string City { get; set; } = null!;
    }
}
