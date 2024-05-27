using Domain.DTOs.BookingDTOs;
using Domain.Filters;
using Infrastructure.Services.BookingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController(IBookingService BookingService) : ControllerBase
    {
        [HttpGet("Bookings")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetBookings([FromQuery]PaginationFilter filter)
        {
            var res1 = await BookingService.GetBookingsAsync(filter);
            return StatusCode(res1.StatusCode, res1);
        }

        [HttpGet("{BookingId:int}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetBookingById(int BookingId)
        {
            var res1 = await BookingService.GetBookingByIdAsync(BookingId);
            return StatusCode(res1.StatusCode, res1);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateBooking([FromForm] CreateBookingDto createBooking)
        {
            var result = await BookingService.CreateBookingAsync(createBooking);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateBooking([FromForm] UpdateBookingDto updateBooking)
        {
            var result = await BookingService.UpdateBookingAsync(updateBooking);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{BookingId:int}")]
        public async Task<IActionResult> ChangePassword(int BookingId)
        {
            var result = await BookingService.DeleteBookingAsync(BookingId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
