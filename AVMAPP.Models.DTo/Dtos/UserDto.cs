
using AVMAPP.Models.DTO.Dtos;


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
        [System.Text.Json.Serialization.JsonIgnore]
        public string? ResetPasswordToken { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string? Password { get; set; } // boş bırakılabilir
    }

}
