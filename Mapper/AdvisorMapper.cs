using BackEnd.Dto;
using BackEnd.Entities;

namespace BackEnd.Mapper
{
    public class AdvisorMapper
    {
        public AdvisorDto ToAdvisorDto(User user)
        {
            return new AdvisorDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}
