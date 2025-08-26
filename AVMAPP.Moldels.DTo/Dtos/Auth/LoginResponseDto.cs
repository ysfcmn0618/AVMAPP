using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTo.Dtos.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}
