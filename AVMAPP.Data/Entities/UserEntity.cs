using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Data.Entities
{
    public class UserEntity : IdentityUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? AddressOther { get; set; }
        public string Password { get; set; } = null!;
        public string? ResetPasswordToken { get; set; }
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
        public string? FullName { get; set; }
        public virtual ICollection<OrderEntity>? Orders { get; set; }
        public virtual ICollection<ProductEntity>? Products { get; set; } 
        public virtual ICollection<ProductCommentEntity>? Comments { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;       
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

    }  

}
