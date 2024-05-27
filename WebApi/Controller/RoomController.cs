using Domain.DTOs.RoomDTOs;
using Domain.Filters;
using Infrastructure.Services.RoomService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController(IRoomService RoomService) : ControllerBase
    {
        [HttpGet("Rooms")]
        [Authorize(Roles = "Admin,User,Staff")]
        public async Task<IActionResult> GetRooms([FromQuery] RoomFilter filter)
        {
            var res1 = await RoomService.GetRoomsAsync(filter);
            return StatusCode(res1.StatusCode, res1);
        }

        [HttpGet("{RoomId:int}")]
        [Authorize(Roles = "User,Admin,Staff")]
        public async Task<IActionResult> GetRoomById(int RoomId)
        {
            var res1 = await RoomService.GetRoomByIdAsync(RoomId);
            return StatusCode(res1.StatusCode, res1);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateRoom([FromForm] CreateRoomDto createRoom)
        {
            var result = await RoomService.CreateRoomAsync(createRoom);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateRoom([FromForm] UpdateRoomDto updateRoom)
        {
            var result = await RoomService.UpdateRoomAsync(updateRoom);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{RoomId:int}")]
        public async Task<IActionResult> ChangePassword(int RoomId)
        {
            var result = await RoomService.DeleteRoomAsync(RoomId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
