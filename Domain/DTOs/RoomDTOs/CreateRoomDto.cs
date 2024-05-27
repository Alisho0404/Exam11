using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.RoomDTOs
{
    public class CreateRoomDto
    {
        public required string RoomNumber { get; set; }
        public string? Description { get; set; }
        public Types? Type { get; set; }
        public decimal PricePerNight { get; set; }
        public RoomStatus? Status { get; set; }
        public IFormFile? PhotoPath { get; set; }
    }
}
