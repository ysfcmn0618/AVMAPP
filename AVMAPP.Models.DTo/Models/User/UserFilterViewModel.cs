using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTO.Models.User
{
    public class UserFilterViewModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? RoleId { get; set; }
        public bool? EmailConfirmed { get; set; }
    }

}
