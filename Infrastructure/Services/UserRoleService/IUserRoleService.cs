using Domain.DTOs.UserRoleDTOs;
using Domain.Filters;
using Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.UserRoleService
{
    public interface IUserRoleService
    {
        Task<PagedResponse<List<GetUserRoleDto>>> GetUserRolesAsync(PaginationFilter filter);
        Task<Response<GetUserRoleDto>> GetUserRoleByIdAsync(int id);
        Task<Response<string>> CreateUserRoleAsync(CreateUserRoleDto UserRoleDto);
        Task<Response<string>> UpdateUserRoleAsync(UpdateUserRoleDto UserRoleDto);
        Task<Response<bool>> DeleteUserRoleAsync(int id);
    }
}
