using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.RoomDTOs
{
    public class UpdateRoomDto
    {
        public int Id { get; set; }
        public required string RoomNumber { get; set; }
        public string? Description { get; set; }
        public Types? Type { get; set; }
        public decimal PricePerNight { get; set; }
        public RoomStatus? Status { get; set; }
        public IFormFile? PhotoPath { get; set; }
    }
}
