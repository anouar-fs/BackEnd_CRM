using BackEnd.Dto;

namespace BackEnd.Services.Advisor
{
    public interface IAdvisorService
    {
        public Task<IEnumerable<AdvisorDto>> getAdvisors();
    }
}
