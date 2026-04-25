using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using BackEnd.Mapper;
namespace BackEnd.Repositories.Advisor
{
    public class AdvisorRepository:IAdvisorRepository
    {
        private AppDbContext _dbContext;
        private AdvisorMapper _mapper;

        public AdvisorRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = new AdvisorMapper();
        }
        public async Task<IEnumerable<User>> getAdvisors()
        {
            var Users = _dbContext.Users.Where(a => a.Role == UserRole.Advisor);

            return Users;
        }
    }
}
