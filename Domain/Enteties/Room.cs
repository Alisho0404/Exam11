using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enteties
{
    public class Room:BaseEntity
    {        
        public required string RoomNumber { get; set; }
        public string? Description { get; set; }
        public Types? Type { get; set; }
        public decimal PricePerNight { get; set; }
        public RoomStatus? Status { get; set; }
        public string? PhotoPath { get; set; }        
        public List<Booking>? Bookings { get; set; }

    }
}
