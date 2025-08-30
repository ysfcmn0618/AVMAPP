using AVMAPP.Models.DTo.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTO.Dtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
        // İstersen sonra ekleyebiliriz:
        // public DateTime ExpiresAt { get; set; }
        // public string RefreshToken { get; set; }
    }
}
