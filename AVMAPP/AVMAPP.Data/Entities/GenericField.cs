using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Data.Entities
{
    public abstract class GenericField
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = "System";
        public string UpdatedBy { get; set; } = "System";
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

    }
}
