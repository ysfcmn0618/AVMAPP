using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Data.Entities
{
    internal class UserEntity : IdentityUser, IGenericField
    {
        public string? FullName { get; set; }

        public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;       
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }

}
