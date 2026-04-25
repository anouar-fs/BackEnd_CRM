using BackEnd.Entities;

namespace BackEnd.Repositories.Advisor
{
    public interface IAdvisorRepository
    {
        public Task<IEnumerable<User>> getAdvisors();
    }
}
