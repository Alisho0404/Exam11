using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enteties
{
    public class Booking:BaseEntity
    {       
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; } 
        public DateTime CheckOutDate { get; set; }
        public BookStatus? BookingStatus { get; set; }
        public User? User { get; set; }
        public Room? Room { get; set; }

    }
    
}
