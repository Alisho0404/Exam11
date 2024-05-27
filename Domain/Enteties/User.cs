using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enteties
{
    public class User:BaseEntity
    {
        
        public required string Name { get; set; }
        public string? Email { get; set; }
        public required string Password { get; set; }               
        public string? Code { get; set; }
        public DateTimeOffset CodeTime { get; set; }
        public List<Booking>? Booking { get; set; }
        public List<Payment>?Payment { get; set; }

        public List<UserRole>? UserRoles { get; set;}
        
    }
}
