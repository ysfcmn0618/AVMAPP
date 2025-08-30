using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Data.Entities
{
    public class RoleEntity 
    {
        public int Id { get; set; } 
        public string Name { get; set; } = "Undefined";
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<UserRoleEntity>? UserRoles { get; set; } = new List<UserRoleEntity>();
        private string _normalizedName = "UNDEFINED";
        public string NormalizedName {
            get => _normalizedName; 
            internal set {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _normalizedName = "UNDEFINED";
                    return;
                }

                // Fazla boşlukları temizle ve büyük harfe çevir
                var cleaned = string.Join(" ",
                    value.Trim()
                         .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                         .Select(w => w.ToUpper())
                );

                _normalizedName = cleaned;
            } }
    }
}
