using BackEnd.Mapper;
using BackEnd.Repositories;
using BackEnd.Repositories.Advisor;

namespace BackEnd.Services.Advisor
{
    public class AdvisorService:IAdvisorService
    {
        private IAdvisorRepository _advisorRepository { get; set; }
        private AdvisorMapper _advisorMapper { get; set; }

        public AdvisorService(IAdvisorRepository advisorRepository, AdvisorMapper advisorMapper)
        {
            _advisorRepository = advisorRepository;
            _advisorMapper = advisorMapper;
        }

        public async Task<IEnumerable<AdvisorDto>> getAdvisors()
        {
            var advisors = new List<AdvisorDto>();
            var userAdvisors = await _advisorRepository.getAdvisors();
            foreach (var userAdvisor in userAdvisors)
            {
                advisors.Add(_advisorMapper.ToAdvisorDto(userAdvisor)); 
            }

            return advisors;
        }
    }
}
