using Domain.DTOs.PaymentDTOs;
using Domain.Enteties;
using Domain.Enums;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.PaymentService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Services.PaymentService
{
    public class PaymentService(ILogger<PaymentService> logger, DataContext context) : IPaymentService
    {

        public async Task<Response<string>> CreatePaymentAsync(CreatePaymentDto PaymentDto)
        {
            try
            {
                logger.LogInformation("Starting method {CreatePaymentAsync} in time:{DateTime}", "CreatePaymentAsync",
                    DateTimeOffset.UtcNow);
                var newPayment = new Payment()
                {
                    UserId = PaymentDto.UserId,
                    BookingId = PaymentDto.BookingId,
                    Amount = PaymentDto.Amount,
                    Date = PaymentDto.Date,
                    PaymentStatus = PaymentDto.PaymentStatus,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                };
                await context.Payments.AddAsync(newPayment);
                await context.SaveChangesAsync();

                logger.LogInformation("Finished method {CreatePaymentAsync} in time:{DateTime}", "CreatePaymentAsync",
                    DateTimeOffset.UtcNow);
                return new Response<string>($"Succesfully created by id {newPayment.Id}");
            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);

            }
        }

        public async Task<Response<bool>> DeletePaymentAsync(int id)
        {
            try
            {
                var existing = await context.Payments.FirstOrDefaultAsync(x => x.Id == id);
                if (existing == null)
                {
                    return new Response<bool>(HttpStatusCode.BadRequest, "Not found");
                }

                context.Payments.Remove(existing);
                await context.SaveChangesAsync();

                logger.LogInformation("Finished method {DeletePaymentAsync} in time:{DateTime}", "DeletePaymentAsync",
                    DateTimeOffset.UtcNow);
                return new Response<bool>(true);
            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);

            }

        }

        public async Task<Response<GetPaymentDto>> GetPaymentByIdAsync(int id)
        {
            try
            {
                logger.LogInformation("Starting method {GetPaymentByIdAsync} in time:{DateTime} ", "GetPaymentByIdAsync",
                DateTimeOffset.UtcNow);

                var book = await context.Payments.Select(x => new GetPaymentDto()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    BookingId = x.BookingId,
                    Amount = x.Amount,
                    Date = x.Date,
                    PaymentStatus = x.PaymentStatus,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                }).FirstOrDefaultAsync();

                if (book is null)
                {
                    logger.LogWarning("Not found Payment with id={id},time={DateTimeNow}", id, DateTime.UtcNow);
                    return new Response<GetPaymentDto>(HttpStatusCode.BadRequest, "Not found");
                }

                logger.LogInformation("Finished method {GetPaymentsAsync} in time {DateTime}", "GetPaymentsAsync",
                    DateTimeOffset.UtcNow);
                return new Response<GetPaymentDto>(book);
            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<GetPaymentDto>(HttpStatusCode.InternalServerError, e.Message);

            }
        }

        public async Task<PagedResponse<List<GetPaymentDto>>> GetPaymentsAsync(PaginationFilter filter)
        {
            try
            {
                logger.LogInformation("Starting methods {GetPaymentsAsync} in time: {DateTime}", "GetPaymentsAsync",
                    DateTimeOffset.UtcNow);

                var books = context.Payments.AsQueryable();
                var response = await books.Select(x => new GetPaymentDto()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    BookingId = x.BookingId,
                    Amount = x.Amount,
                    Date = x.Date,
                    PaymentStatus = x.PaymentStatus,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow

                }).Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize).ToListAsync();

                var totalRecord = await books.CountAsync();

                logger.LogInformation("Finished method {GetPaymentsAsync} in time: {DateTime}", "GetPaymentsAsync",
                    DateTimeOffset.UtcNow);

                return new PagedResponse<List<GetPaymentDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);

            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new PagedResponse<List<GetPaymentDto>>(HttpStatusCode.InternalServerError, e.Message);

            }
        }

        public async Task<Response<string>> UpdatePaymentAsync(UpdatePaymentDto PaymentDto)
        {
            try
            {
                logger.LogInformation("Starting method {UpdatePaymentAsync} in time:{DateTime} ", "UpdatePaymentAsync",
                DateTimeOffset.UtcNow);
                var existing = await context.Payments.FirstOrDefaultAsync(x => x.Id == PaymentDto.Id);
                if (existing is null)
                {
                    logger.LogWarning("Payment not found by id:{Id},time:{DateTimeNow} ", PaymentDto.Id,
                        DateTimeOffset.UtcNow);
                    return new Response<string>(HttpStatusCode.BadRequest, "Payment not found");
                }
                existing.Id = PaymentDto.Id;
                existing.UserId = PaymentDto.UserId;
                existing.BookingId = PaymentDto.BookingId;
                existing.Amount = PaymentDto.Amount;
                existing.Date = PaymentDto.Date;
                existing.PaymentStatus = PaymentDto.PaymentStatus;
                existing.CreatedAt = DateTimeOffset.UtcNow;
                existing.UpdatedAt = DateTimeOffset.UtcNow;

                await context.SaveChangesAsync();
                logger.LogInformation("Finished method {UpdatePaymentAsync} in time:{DateTime}", "UpdatePaymentAsync",
                    DateTimeOffset.UtcNow);
                return new Response<string>($"Succesfully updated by id:{PaymentDto.Id}");

            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);

            }
        }
    }
}
