using Domain.DTOs.UserDTOs;
using Domain.Filters;
using Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.UserService
{
    public interface IUserService
    {
        Task<PagedResponse<List<GetUserDto>>> GetUsersAsync(UserFilter filter);
        Task<Response<GetUserDto>> GetUserByIdAsync(int userId);
        Task<Response<string>> UpdateUserAsync(UpdateUserDto updateUser);
    }
}
