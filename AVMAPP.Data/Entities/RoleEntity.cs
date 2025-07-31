using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Data.Entities
{
    public class RoleEntity : IdentityRole
    {
        public int Id { get; set; }
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
        // IGenericField alanları
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }
}
