using AVMAPP.Models.DTo.Dtos;


namespace AVMAPP.Models.DTO.Models.User
{
    public class UserListViewModel
    {
        public List<UserDto> Users { get; set; } = new();
        public UserFilterViewModel Filter { get; set; } = new(); 
    }

}
