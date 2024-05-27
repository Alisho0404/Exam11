using Domain.DTOs.RoomDTOs;
using Domain.Filters;
using Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.RoomService
{
    public interface IRoomService
    {
        Task<PagedResponse<List<GetRoomDto>>> GetRoomsAsync(RoomFilter filter);
        Task<Response<GetRoomDto>> GetRoomByIdAsync(int id);
        Task<Response<string>> CreateRoomAsync(CreateRoomDto RoomDto);
        Task<Response<string>> UpdateRoomAsync(UpdateRoomDto RoomDto);
        Task<Response<bool>> DeleteRoomAsync(int id);
    }
}
