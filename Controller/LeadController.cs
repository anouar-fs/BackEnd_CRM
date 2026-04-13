using BackEnd.Entities;
using BackEnd.Services.Lead;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using BackEnd.Dto;


namespace BackEnd.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadController : ControllerBase
    {
        private ILeadService _leadService;

        public LeadController(ILeadService leadService)
        {
            _leadService = leadService;
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<LeadDto>> AddUser([FromBody] LeadDto lead)
        {
            try
            {
                var ld =  await _leadService.createLead(lead);
                return Ok(ld);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<LeadsResponse>> GetLeads(int page,int pageSize)
        {
            try
            {
                var leads = await _leadService.getLeads(page,pageSize);
                return Ok(leads);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeadsResponse>> GetLead(int id)
        {
            try
            {
                var lead = await _leadService.getLeadById(id);
                return Ok(lead);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("/Jid/{whatsappJid}")]
        public async Task<ActionResult<LeadsResponse>> GetLeadByJid(string whatsappJid)
        {
            try
            {
                var lead = await _leadService.GetLeadByJid(whatsappJid);
                return Ok(lead);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("/lead/stats")]
        public async Task<IActionResult> getLeadsStats()
        {
            try
            {
                var leadStats = _leadService.getLeadStats();
                return Ok(leadStats);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        [Consumes("application/json-patch+json")]
        public async Task<IActionResult> UpdateLead(
            int id,
            [FromBody] JsonPatchDocument<Lead> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            try
            {
                await _leadService.updateAsync(id, patchDoc);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }


            return NoContent();
        }

    }
}
