using Domain.DTOs.BookingDTOs;
using Domain.Filters;
using Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.BookingService
{
    public interface IBookingService
    {
        Task<PagedResponse<List<GetBookingDto>>> GetBookingsAsync(PaginationFilter filter);
        Task<Response<GetBookingDto>> GetBookingByIdAsync(int id);
        Task<Response<string>> CreateBookingAsync(CreateBookingDto bookingDto);
        Task<Response<string>> UpdateBookingAsync(UpdateBookingDto bookingDto); 
        Task<Response<bool>>DeleteBookingAsync(int id);
    }
}
