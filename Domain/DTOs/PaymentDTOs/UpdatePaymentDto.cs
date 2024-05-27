using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.PaymentDTOs
{
    public class UpdatePaymentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
    }
}
