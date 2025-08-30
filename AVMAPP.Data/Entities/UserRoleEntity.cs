using Microsoft.AspNetCore.Identity;

namespace AVMAPP.Data.Entities
{
    public class UserRoleEntity
    {
        public Guid UserId { get; set; }
        public  UserEntity User { get; set; } = null!;
        public int RoleId { get; set; }
        public  RoleEntity Role { get; set; } = null!;
    }
}