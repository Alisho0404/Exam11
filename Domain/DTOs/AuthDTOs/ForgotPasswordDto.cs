using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.AuthDTOs
{
    public class ForgotPasswordDto
    {
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }
    }
}
