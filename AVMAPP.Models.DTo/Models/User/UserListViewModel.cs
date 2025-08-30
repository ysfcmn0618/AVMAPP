using AVMAPP.Models.DTo.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTO.Models.User
{
    public class UserListViewModel
    {
        public List<UserDto> Users { get; set; } = new();
        public UserFilterViewModel Filter { get; set; } = new(); // ileride filtreleme ekleyebiliriz
    }

}
