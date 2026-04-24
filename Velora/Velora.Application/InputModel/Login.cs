using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Velora.Application.InputModel
{
    public class Login
    {

        [EmailAddress]
        public string Email { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
