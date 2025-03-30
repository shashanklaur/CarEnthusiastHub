using System.Collections.Generic;

namespace CarEnthusiastHub.Models
{
    public class Car
    {
        public int CarId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string ImageUrl { get; set; }
        public string Review { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<CarCategory> CarCategories { get; set; }
    }
}
