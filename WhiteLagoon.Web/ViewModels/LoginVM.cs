using System.ComponentModel.DataAnnotations;
namespace WhiteLagoon.Web.ViewModels
{
    public class LoginVM
    {
        [Required]
        public required string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        public bool RememberMe { get; set; }
        public string? RedirectUrl { get; set; }
    }
}