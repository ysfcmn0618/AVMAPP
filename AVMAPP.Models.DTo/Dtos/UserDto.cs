using AVMAPP.Data.APi.Models;
using AVMAPP.Data.APi.Models.Dtos;
using AVMAPP.Data.Entities;
using AVMAPP.Models.DTO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTo.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = "Undefined";
        public string LastName { get; set; } = "Undefined";
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int RoleId { get; set; }

        // Eklenen alan
        public bool EmailConfirmed { get; set; } = false;

        public RoleDto? Role { get; set; }
        public string? ResetPasswordToken { get; set; }
        public string? Password { get; set; } // boş bırakılabilir
    }

}
