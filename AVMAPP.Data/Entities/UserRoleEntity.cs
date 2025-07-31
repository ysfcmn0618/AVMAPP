using Microsoft.AspNetCore.Identity;

namespace AVMAPP.Data.Entities
{
    public class UserRoleEntity 
    {
        public int Id { get; set; } 
        public int RoleId { get; set; }
        public int UserId { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual RoleEntity Role { get; set; }
    }
}