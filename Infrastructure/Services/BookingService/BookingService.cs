using Domain.DTOs.BookingDTOs;
using Domain.Enteties;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Infrastructure.Services.BookingService
{
    public class BookingService(ILogger<BookingService> logger, DataContext context) : IBookingService
    {

        public async Task<Response<string>> CreateBookingAsync(CreateBookingDto bookingDto)
        {
            try
            {
                logger.LogInformation("Starting method {CreateBookingAsync} in time:{DateTime}", "CreateBookingAsync",
                    DateTimeOffset.UtcNow);
                var newBooking = new Booking()
                {
                    UserId = bookingDto.UserId,
                    RoomId = bookingDto.RoomId,
                    CheckInDate = bookingDto.CheckInDate,
                    CheckOutDate = bookingDto.CheckOutDate,
                    BookingStatus = bookingDto.BookingStatus,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                };
                await context.Bookings.AddAsync(newBooking);
                await context.SaveChangesAsync();

                logger.LogInformation("Finished method {CreateBookingAsync} in time:{DateTime}", "CreateBookingAsync",
                    DateTimeOffset.UtcNow);
                return new Response<string>($"Succesfully created by id {newBooking.Id}");
            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);

            }
        }

        public async Task<Response<bool>> DeleteBookingAsync(int id)
        {
            try
            {
                var existing = await context.Bookings.FirstOrDefaultAsync(x => x.Id == id);
                if (existing == null)
                {
                    return new Response<bool>(HttpStatusCode.BadRequest, "Not found");
                }

                context.Bookings.Remove(existing);
                await context.SaveChangesAsync();

                logger.LogInformation("Finished method {DeleteBookingAsync} in time:{DateTime}", "DeleteBookingAsync",
                    DateTimeOffset.UtcNow);
                return new Response<bool>(true);
            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);

            }

        }

        public async Task<Response<GetBookingDto>> GetBookingByIdAsync(int id)
        {
            try
            {
                logger.LogInformation("Starting method {GetBookingByIdAsync} in time:{DateTime} ", "GetBookingByIdAsync",
                DateTimeOffset.UtcNow);

                var book = await context.Bookings.Select(x => new GetBookingDto()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    RoomId = x.RoomId,
                    CheckInDate = x.CheckInDate,
                    CheckOutDate = x.CheckOutDate,
                    BookingStatus = x.BookingStatus,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).FirstOrDefaultAsync();

                if (book is null)
                {
                    logger.LogWarning("Not found Booking with id={id},time={DateTimeNow}", id, DateTime.UtcNow);
                    return new Response<GetBookingDto>(HttpStatusCode.BadRequest, "Not found");
                }

                logger.LogInformation("Finished method {GetBookingsAsync} in time {DateTime}", "GetBookingsAsync",
                    DateTimeOffset.UtcNow);
                return new Response<GetBookingDto>(book);
            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<GetBookingDto>(HttpStatusCode.InternalServerError, e.Message);

            }
        }

        public async Task<PagedResponse<List<GetBookingDto>>> GetBookingsAsync(PaginationFilter filter)
        {
            try
            {
                logger.LogInformation("Starting methods {GetBookingsAsync} in time: {DateTime}", "GetBookingsAsync",
                    DateTimeOffset.UtcNow);

                var books = context.Bookings.AsQueryable();
                var response = await books.Select(x => new GetBookingDto()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    RoomId = x.RoomId,
                    CheckInDate = x.CheckInDate,
                    CheckOutDate = x.CheckOutDate,
                    BookingStatus = x.BookingStatus,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize).ToListAsync();

                var totalRecord = await books.CountAsync();

                logger.LogInformation("Finished method {GetBookingsAsync} in time: {DateTime}", "GetBookingsAsync",
                    DateTimeOffset.UtcNow);

                return new PagedResponse<List<GetBookingDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);

            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new PagedResponse<List<GetBookingDto>>(HttpStatusCode.InternalServerError, e.Message);

            }
        }

        public async Task<Response<string>> UpdateBookingAsync(UpdateBookingDto bookingDto)
        {
            try
            {
                logger.LogInformation("Starting method {UpdateBookingAsync} in time:{DateTime} ", "UpdateBookingAsync",
                DateTimeOffset.UtcNow);
                var existing = await context.Bookings.FirstOrDefaultAsync(x => x.Id == bookingDto.Id);
                if (existing is null)
                {
                    logger.LogWarning("Booking not found by id:{Id},time:{DateTimeNow} ", bookingDto.Id,
                        DateTimeOffset.UtcNow);
                    return new Response<string>(HttpStatusCode.BadRequest, "Booking not found");
                }
                existing.Id = bookingDto.Id;
                existing.UserId = bookingDto.UserId;
                existing.RoomId = bookingDto.RoomId;
                existing.CheckInDate = bookingDto.CheckInDate;
                existing.CheckOutDate = bookingDto.CheckOutDate;
                existing.BookingStatus = bookingDto.BookingStatus;
                existing.UpdatedAt = DateTimeOffset.UtcNow;

                await context.SaveChangesAsync();
                logger.LogInformation("Finished method {UpdateBookingAsync} in time:{DateTime}", "UpdateBookingAsync",
                    DateTimeOffset.UtcNow);
                return new Response<string>($"Succesfully updated by id:{bookingDto.Id}");

            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
                
            }
        }
    }
}
