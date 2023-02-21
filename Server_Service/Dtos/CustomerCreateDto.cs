using System.ComponentModel.DataAnnotations;

namespace Server_Service.Dtos
{
    public class CustomerCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string City { get; set; }
    }
}
