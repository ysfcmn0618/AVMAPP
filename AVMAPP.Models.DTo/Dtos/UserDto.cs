using AVMAPP.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTo.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool EmailConfirmed { get; set; } = true;
        public string? PasswordHash { get; set; } = string.Empty;
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public string? Address { get; set; } = null!;
        public string? AddressOther { get; set; }
        public string Password { get; set; }
        public string? ResetPasswordToken { get; set; }
        public Guid RoleId { get; set; }
        public RoleEntity Role { get; set; } = null!;
        public string? FullName { get; set; }
        public virtual ICollection<OrderEntity>? Orders { get; set; }
        public virtual ICollection<ProductEntity>? Products { get; set; }
        public virtual ICollection<ProductCommentEntity>? Comments { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? SecurityStamp { get; set; }
        public ICollection<UserRoleEntity>? UserRoles { get; set; } = new List<UserRoleEntity>();
    }
}
