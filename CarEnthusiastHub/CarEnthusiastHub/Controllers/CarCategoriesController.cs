using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CarEnthusiastHub.Models;

namespace CarEnthusiastHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarCategoriesController : ControllerBase
    {
        private readonly CarEnthusiastDbContext _context;

        public CarCategoriesController(CarEnthusiastDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<CarCategory>> AddCarToCategory(CarCategory carCategory)
        {
            _context.CarCategories.Add(carCategory);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCarCategories), new { carId = carCategory.CarId, categoryId = carCategory.CategoryId }, carCategory);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarCategory>>> GetCarCategories()
        {
            return await _context.CarCategories
                .Include(cc => cc.Car)
                .Include(cc => cc.Category)
                .ToListAsync();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveCarFromCategory(int carId, int categoryId)
        {
            var carCategory = await _context.CarCategories.FindAsync(carId, categoryId);
            if (carCategory == null)
                return NotFound();

            _context.CarCategories.Remove(carCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
