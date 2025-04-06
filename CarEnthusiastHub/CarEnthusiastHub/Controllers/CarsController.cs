using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CarEnthusiastHub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace CarEnthusiastHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CarsController : ControllerBase
    {
        private readonly CarEnthusiastDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CarsController(CarEnthusiastDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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

        //  New Endpoint: Create car with image upload
        [HttpPost("create-with-image")]
        public async Task<IActionResult> CreateCarWithImage([FromForm] Car formCar, IFormFile imageFile)
        {
            var car = new Car
            {
                Make = formCar.Make,
                Model = formCar.Model,
                Year = formCar.Year,
                Review = formCar.Review,
                UserId = formCar.UserId,
                // Don't assign CarId
            };

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                car.ImagePath = "/uploads/" + fileName;
            }

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

