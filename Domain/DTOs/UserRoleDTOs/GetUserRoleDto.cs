using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.UserRoleDTOs
{
    public class GetUserRoleDto
    {
        public int Id { get; set; }        
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
