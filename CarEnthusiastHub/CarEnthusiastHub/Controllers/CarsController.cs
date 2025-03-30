using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CarEnthusiastHub.Models;

namespace CarEnthusiastHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarsController : ControllerBase
    {
        private readonly CarEnthusiastDbContext _context;

        public CarsController(CarEnthusiastDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            return await _context.Cars
                .Include(c => c.CarCategories)
                .ThenInclude(cc => cc.Category)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(CarDTO carDTO)
        {
            var car = new Car
            {
                Make = carDTO.Make,
                Model = carDTO.Model,
                Year = carDTO.Year,
                ImageUrl = carDTO.ImageUrl,
                Review = carDTO.Review,
                UserId = carDTO.UserId
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCars), new { id = car.CarId }, car);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(int id, CarDTO carDTO)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            car.Make = carDTO.Make;
            car.Model = carDTO.Model;
            car.Year = carDTO.Year;
            car.ImageUrl = carDTO.ImageUrl;
            car.Review = carDTO.Review;
            car.UserId = carDTO.UserId;

            _context.Entry(car).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
                return NotFound();

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
