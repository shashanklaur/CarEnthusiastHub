using System.Collections.Generic;

namespace CarEnthusiastHub.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? Salt { get; set; }
        public ICollection<Car>? Cars { get; set; }
    }
}
