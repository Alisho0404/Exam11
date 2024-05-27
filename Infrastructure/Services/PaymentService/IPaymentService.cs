using Domain.DTOs.PaymentDTOs;
using Domain.Filters;
using Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<PagedResponse<List<GetPaymentDto>>> GetPaymentsAsync(PaginationFilter filter);
        Task<Response<GetPaymentDto>> GetPaymentByIdAsync(int id);
        Task<Response<string>> CreatePaymentAsync(CreatePaymentDto PaymentDto);
        Task<Response<string>> UpdatePaymentAsync(UpdatePaymentDto PaymentDto);
        Task<Response<bool>> DeletePaymentAsync(int id);
    }
}
