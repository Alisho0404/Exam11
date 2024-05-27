using Domain.DTOs.RoleDTOs;
using Domain.Filters;
using Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.RoleService
{
    public interface IRoleService
    {
        Task<PagedResponse<List<GetRoleDto>>> GetRolesAsync(PaginationFilter filter);
        Task<Response<GetRoleDto>> GetRoleByIdAsync(int id);
        Task<Response<string>> CreateRoleAsync(CreateRoleDto RoleDto);
        Task<Response<string>> UpdateRoleAsync(UpdateRoleDto RoleDto);
        Task<Response<bool>> DeleteRoleAsync(int id);
    }
}
