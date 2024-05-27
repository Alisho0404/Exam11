using Domain.DTOs.RoomDTOs;
using Domain.Enteties;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Infrastructure.Services.RoomService
{
    public class RoomService(IFileService fileService,ILogger<RoomService> logger, DataContext context) : IRoomService
    {

        public async Task<Response<string>> CreateRoomAsync(CreateRoomDto RoomDto)
        {
            try
            {
                logger.LogInformation("Starting method {CreateRoomAsync} in time:{DateTime}", "CreateRoomAsync",
                    DateTimeOffset.UtcNow);
                var newRoom = new Room()
                {
                    RoomNumber = RoomDto.RoomNumber,
                    Description = RoomDto.Description,
                    Type = RoomDto.Type,
                    PricePerNight = RoomDto.PricePerNight,
                    Status = RoomDto.Status,
                    PhotoPath = RoomDto.PhotoPath == null ? "null" : await fileService.CreateFile(RoomDto.PhotoPath),
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                };
                await context.Rooms.AddAsync(newRoom);
                await context.SaveChangesAsync();

                logger.LogInformation("Finished method {CreateRoomAsync} in time:{DateTime}", "CreateRoomAsync",
                    DateTimeOffset.UtcNow);
                return new Response<string>($"Succesfully created by id {newRoom.Id}");
            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);

            }
        }

        public async Task<Response<bool>> DeleteRoomAsync(int id)
        {
            try
            {
                var existing = await context.Rooms.FirstOrDefaultAsync(x => x.Id == id);
                if (existing == null)
                {
                    return new Response<bool>(HttpStatusCode.BadRequest, "Not found");
                }

                context.Rooms.Remove(existing);
                await context.SaveChangesAsync();

                logger.LogInformation("Finished method {DeleteRoomAsync} in time:{DateTime}", "DeleteRoomAsync",
                    DateTimeOffset.UtcNow);
                return new Response<bool>(true);
            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);

            }

        }

        public async Task<Response<GetRoomDto>> GetRoomByIdAsync(int id)
        {
            try
            {
                logger.LogInformation("Starting method {GetRoomByIdAsync} in time:{DateTime} ", "GetRoomByIdAsync",
                DateTimeOffset.UtcNow);

                var book = await context.Rooms.Select(x => new GetRoomDto()
                {
                    Id = x.Id,
                    RoomNumber = x.RoomNumber,
                    Description = x.Description,
                    Type = x.Type,
                    PricePerNight = x.PricePerNight,
                    Status = x.Status,
                    PhotoPath = x.PhotoPath,                    
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).FirstOrDefaultAsync();

                if (book is null)
                {
                    logger.LogWarning("Not found Room with id={id},time={DateTimeNow}", id, DateTime.UtcNow);
                    return new Response<GetRoomDto>(HttpStatusCode.BadRequest, "Not found");
                }

                logger.LogInformation("Finished method {GetRoomsAsync} in time {DateTime}", "GetRoomsAsync",
                    DateTimeOffset.UtcNow);
                return new Response<GetRoomDto>(book);
            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<GetRoomDto>(HttpStatusCode.InternalServerError, e.Message);

            }
        }

        public async Task<PagedResponse<List<GetRoomDto>>> GetRoomsAsync(RoomFilter filter)
        {
            try
            {
                logger.LogInformation("Starting methods {GetRoomsAsync} in time: {DateTime}", "GetRoomsAsync",
                    DateTimeOffset.UtcNow);

                var books = context.Rooms.AsQueryable();
                if (!string.IsNullOrEmpty(filter.RoomNumber))
                {
                    books = books.Where(x => x.RoomNumber.Contains(filter.RoomNumber));
                }
                var response = await books.Select(x => new GetRoomDto()
                {
                    Id = x.Id,
                    RoomNumber = x.RoomNumber,
                    Description = x.Description,
                    Type = x.Type,
                    PricePerNight = x.PricePerNight,
                    Status = x.Status,
                    PhotoPath = x.PhotoPath,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize).ToListAsync();

                var totalRecord = await books.CountAsync();

                logger.LogInformation("Finished method {GetRoomsAsync} in time: {DateTime}", "GetRoomsAsync",
                    DateTimeOffset.UtcNow);

                return new PagedResponse<List<GetRoomDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);

            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new PagedResponse<List<GetRoomDto>>(HttpStatusCode.InternalServerError, e.Message);

            }
        }

        public async Task<Response<string>> UpdateRoomAsync(UpdateRoomDto RoomDto)
        {
            try
            {
                logger.LogInformation("Starting method {UpdateRoomAsync} in time:{DateTime} ", "UpdateRoomAsync",
                DateTimeOffset.UtcNow);
                var existing = await context.Rooms.FirstOrDefaultAsync(x => x.Id == RoomDto.Id);
                if (existing is null)
                {
                    logger.LogWarning("Room not found by id:{Id},time:{DateTimeNow} ", RoomDto.Id,
                        DateTimeOffset.UtcNow);
                    return new Response<string>(HttpStatusCode.BadRequest, "Room not found");
                }
                existing.Id = RoomDto.Id;
                existing.RoomNumber = RoomDto.RoomNumber;
                existing.Description = RoomDto.Description;
                existing.Type = RoomDto.Type;
                existing.PricePerNight = RoomDto.PricePerNight;
                existing.Status = RoomDto.Status; 

                if (RoomDto.PhotoPath is not null)
                {
                    if (existing.PhotoPath!=null)
                    {
                        fileService.DeleteFile(existing.PhotoPath);
                    }
                    existing.PhotoPath = await fileService.CreateFile(RoomDto.PhotoPath);
                }
               
                existing.UpdatedAt = DateTimeOffset.UtcNow;

                await context.SaveChangesAsync();
                logger.LogInformation("Finished method {UpdateRoomAsync} in time:{DateTime}", "UpdateRoomAsync",
                    DateTimeOffset.UtcNow);
                return new Response<string>($"Succesfully updated by id:{RoomDto.Id}");

            }
            catch (Exception e)
            {
                logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);

            }
        }
    }
}
