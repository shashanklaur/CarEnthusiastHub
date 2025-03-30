using System.Collections.Generic;

namespace CarEnthusiastHub.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<CarCategory> CarCategories { get; set; }
    }
}
