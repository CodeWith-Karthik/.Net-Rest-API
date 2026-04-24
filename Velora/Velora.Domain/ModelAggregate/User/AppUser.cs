using Microsoft.AspNetCore.Identity;
using Velora.Domain.ModelAggregate.Sales;

namespace Velora.Domain.ModelAggregate.User
{
    public class AppUser : IdentityUser<Guid>
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public virtual IEnumerable<Order> Orders { get; set; }
    }
}
