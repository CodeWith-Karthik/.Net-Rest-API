using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Velora.Application.InputModel
{
    public class Register
    {
        [Required]
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateOnly DateOfBirth { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
