using Domain.DTOs.RoleDTOs;
using Domain.Enteties;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Infrastructure.Services.RoleService
{
    public class RoleService(ILogger<RoleService> logger, DataContext context) : IRoleService
    {

        public async Task<Response<string>> CreateRoleAsync(CreateRoleDto RoleDto)
        {
            try
            {
                logger.LogInformation("Starting method {CreateRoleAsync} in time:{DateTime}", "CreateRoleAsync",
                    DateTimeOffset.UtcNow);
                var newRole = new Role()
                {
                    Name=RoleDto.Name,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                };
                await context.Roles.AddAsync(newRole);
                await context.SaveChangesAsync();

                logger.LogInformation("Finished method {CreateRoleAsync} in time:{DateTime}", "CreateRoleAsync",
                    DateTimeOffset.UtcNow);
                return new Response<string>($"Succesfully created by id {newRole.Id}");
            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);

            }
        }

        public async Task<Response<bool>> DeleteRoleAsync(int id)
        {
            try
            {
                var existing = await context.Roles.FirstOrDefaultAsync(x => x.Id == id);
                if (existing == null)
                {
                    return new Response<bool>(HttpStatusCode.BadRequest, "Not found");
                }

                context.Roles.Remove(existing);
                await context.SaveChangesAsync();

                logger.LogInformation("Finished method {DeleteRoleAsync} in time:{DateTime}", "DeleteRoleAsync",
                    DateTimeOffset.UtcNow);
                return new Response<bool>(true);
            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);

            }

        }

        public async Task<Response<GetRoleDto>> GetRoleByIdAsync(int id)
        {
            try
            {
                logger.LogInformation("Starting method {GetRoleByIdAsync} in time:{DateTime} ", "GetRoleByIdAsync",
                DateTimeOffset.UtcNow);

                var book = await context.Roles.Select(x => new GetRoleDto()
                {
                    Id = x.Id,
                    Name=x.Name,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).FirstOrDefaultAsync();

                if (book is null)
                {
                    logger.LogWarning("Not found Role with id={id},time={DateTimeNow}", id, DateTime.UtcNow);
                    return new Response<GetRoleDto>(HttpStatusCode.BadRequest, "Not found");
                }

                logger.LogInformation("Finished method {GetRolesAsync} in time {DateTime}", "GetRolesAsync",
                    DateTimeOffset.UtcNow);
                return new Response<GetRoleDto>(book);
            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<GetRoleDto>(HttpStatusCode.InternalServerError, e.Message);

            }
        }

        public async Task<PagedResponse<List<GetRoleDto>>> GetRolesAsync(PaginationFilter filter)
        {
            try
            {
                logger.LogInformation("Starting methods {GetRolesAsync} in time: {DateTime}", "GetRolesAsync",
                    DateTimeOffset.UtcNow);

                var books = context.Roles.AsQueryable();
                var response = await books.Select(x => new GetRoleDto()
                {
                    Id = x.Id,
                    Name=x.Name,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize).ToListAsync();

                var totalRecord = await books.CountAsync();

                logger.LogInformation("Finished method {GetRolesAsync} in time: {DateTime}", "GetRolesAsync",
                    DateTimeOffset.UtcNow);

                return new PagedResponse<List<GetRoleDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);

            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new PagedResponse<List<GetRoleDto>>(HttpStatusCode.InternalServerError, e.Message);

            }
        }

        public async Task<Response<string>> UpdateRoleAsync(UpdateRoleDto RoleDto)
        {
            try
            {
                logger.LogInformation("Starting method {UpdateRoleAsync} in time:{DateTime} ", "UpdateRoleAsync",
                DateTimeOffset.UtcNow);
                var existing = await context.Roles.FirstOrDefaultAsync(x => x.Id == RoleDto.Id);
                if (existing is null)
                {
                    logger.LogWarning("Role not found by id:{Id},time:{DateTimeNow} ", RoleDto.Id,
                        DateTimeOffset.UtcNow);
                    return new Response<string>(HttpStatusCode.BadRequest, "Role not found");
                }
                existing.Id = RoleDto.Id;
                existing.Name=RoleDto.Name;
                existing.UpdatedAt = DateTimeOffset.UtcNow;

                await context.SaveChangesAsync();
                logger.LogInformation("Finished method {UpdateRoleAsync} in time:{DateTime}", "UpdateRoleAsync",
                    DateTimeOffset.UtcNow);
                return new Response<string>($"Succesfully updated by id:{RoleDto.Id}");

            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);

            }
        }
    }
}
