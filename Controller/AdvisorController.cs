using BackEnd.Services.Advisor;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvisorController : ControllerBase
    {
        private IAdvisorService _advisorService;

        public AdvisorController(IAdvisorService advisorService)
        {
            _advisorService = advisorService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdvisorDto>>> getAdvisors()
        {
            try
            {
                var advisors = await _advisorService.getAdvisors();
                return Ok(advisors);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
