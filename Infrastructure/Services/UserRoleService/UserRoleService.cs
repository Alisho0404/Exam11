using Domain.DTOs.UserRoleDTOs;
using Domain.Enteties;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Infrastructure.Services.UserRoleService
{
    public class UserRoleService(ILogger<UserRoleService> logger, DataContext context) : IUserRoleService
    {

        public async Task<Response<string>> CreateUserRoleAsync(CreateUserRoleDto UserRoleDto)
        {
            try
            {
                logger.LogInformation("Starting method {CreateUserRoleAsync} in time:{DateTime}", "CreateUserRoleAsync",
                    DateTimeOffset.UtcNow);
                var newUserRole = new UserRole()
                {
                    UserId = UserRoleDto.UserId,
                    RoleId = UserRoleDto.RoleId,                    
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                };
                await context.UserRoles.AddAsync(newUserRole);
                await context.SaveChangesAsync();

                logger.LogInformation("Finished method {CreateUserRoleAsync} in time:{DateTime}", "CreateUserRoleAsync",
                    DateTimeOffset.UtcNow);
                return new Response<string>($"Succesfully created by id {newUserRole.Id}");
            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);

            }
        }

        public async Task<Response<bool>> DeleteUserRoleAsync(int id)
        {
            try
            {
                var existing = await context.UserRoles.FirstOrDefaultAsync(x => x.Id == id);
                if (existing == null)
                {
                    return new Response<bool>(HttpStatusCode.BadRequest, "Not found");
                }

                context.UserRoles.Remove(existing);
                await context.SaveChangesAsync();

                logger.LogInformation("Finished method {DeleteUserRoleAsync} in time:{DateTime}", "DeleteUserRoleAsync",
                    DateTimeOffset.UtcNow);
                return new Response<bool>(true);
            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);

            }

        }

        public async Task<Response<GetUserRoleDto>> GetUserRoleByIdAsync(int id)
        {
            try
            {
                logger.LogInformation("Starting method {GetUserRoleByIdAsync} in time:{DateTime} ", "GetUserRoleByIdAsync",
                DateTimeOffset.UtcNow);

                var book = await context.UserRoles.Select(x => new GetUserRoleDto()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    RoleId = x.RoleId,                    
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).FirstOrDefaultAsync();

                if (book is null)
                {
                    logger.LogWarning("Not found UserRole with id={id},time={DateTimeNow}", id, DateTime.UtcNow);
                    return new Response<GetUserRoleDto>(HttpStatusCode.BadRequest, "Not found");
                }

                logger.LogInformation("Finished method {GetUserRolesAsync} in time {DateTime}", "GetUserRolesAsync",
                    DateTimeOffset.UtcNow);
                return new Response<GetUserRoleDto>(book);
            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<GetUserRoleDto>(HttpStatusCode.InternalServerError, e.Message);

            }
        }

        public async Task<PagedResponse<List<GetUserRoleDto>>> GetUserRolesAsync(PaginationFilter filter)
        {
            try
            {
                logger.LogInformation("Starting methods {GetUserRolesAsync} in time: {DateTime}", "GetUserRolesAsync",
                    DateTimeOffset.UtcNow);

                var books = context.UserRoles.AsQueryable();
                var response = await books.Select(x => new GetUserRoleDto()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    RoleId = x.RoleId,                   
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize).ToListAsync();

                var totalRecord = await books.CountAsync();

                logger.LogInformation("Finished method {GetUserRolesAsync} in time: {DateTime}", "GetUserRolesAsync",
                    DateTimeOffset.UtcNow);

                return new PagedResponse<List<GetUserRoleDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);

            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new PagedResponse<List<GetUserRoleDto>>(HttpStatusCode.InternalServerError, e.Message);

            }
        }

        public async Task<Response<string>> UpdateUserRoleAsync(UpdateUserRoleDto UserRoleDto)
        {
            try
            {
                logger.LogInformation("Starting method {UpdateUserRoleAsync} in time:{DateTime} ", "UpdateUserRoleAsync",
                DateTimeOffset.UtcNow);
                var existing = await context.UserRoles.FirstOrDefaultAsync(x => x.Id == UserRoleDto.Id);
                if (existing is null)
                {
                    logger.LogWarning("UserRole not found by id:{Id},time:{DateTimeNow} ", UserRoleDto.Id,
                        DateTimeOffset.UtcNow);
                    return new Response<string>(HttpStatusCode.BadRequest, "UserRole not found");
                }
                existing.Id = UserRoleDto.Id;
                existing.UserId = UserRoleDto.UserId;
                existing.RoleId = UserRoleDto.RoleId;                
                existing.UpdatedAt = DateTimeOffset.UtcNow;

                await context.SaveChangesAsync();
                logger.LogInformation("Finished method {UpdateUserRoleAsync} in time:{DateTime}", "UpdateUserRoleAsync",
                    DateTimeOffset.UtcNow);
                return new Response<string>($"Succesfully updated by id:{UserRoleDto.Id}");

            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);

            }
        }
    }
}
