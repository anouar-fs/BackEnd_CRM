using BackEnd.Entities;
using BackEnd.Services.Lead;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;


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
        public async Task<ActionResult<Lead>> AddUser([FromBody] Lead lead)
        {
            try
            {
                var ld =  await _leadService.createLead(lead);
                return Ok(ld);
            }
            catch (Exception ex)
            {
                return BadRequest(null);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lead>>> GetLeads()
        {
            try
            {
                var leads = await _leadService.getLeads();
                return Ok(leads);
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

            var lead = await _leadService.getLeadById(id);
            if (lead == null)
                return NotFound();

            patchDoc.ApplyTo(lead, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _leadService.updateAsync(lead);

            return NoContent();
        }

    }
}
