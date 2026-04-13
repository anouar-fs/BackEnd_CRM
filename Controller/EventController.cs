using BackEnd.Dto;
using BackEnd.Entities;
using BackEnd.Services.Event;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<Event>> AddEvent([FromBody] EventDto evnt)
        {
            try
            {
                var ev = await _eventService.createEvent(evnt);
                return Ok(ev);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<Event>> GetEventByLead_ConseillerId(int leadId, int ConseillerId)
        {
            try
            {
                var ev = await _eventService.GetEventByConseillerId_leadId(leadId, ConseillerId);
                return Ok(ev);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/Time/Advisor")]
        public async Task<ActionResult<Event>> GetEventByDate_Hour_ConseillerId(TimeOnly time, int ConseillerId, DateTime date)
        {
            try
            {
                var ev = await _eventService.GetEventByConseillerId_leadId(0, ConseillerId);
                return Ok(ev);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/Slot")]
        public async Task<ActionResult<SlotDto>> GetNextSlot()
        {
            try
            {
                var slot = await _eventService.GetNextSlot();

                return Ok(new SlotDto { Date = slot.Date, Slot = slot.Slot });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/availabilities/{date}")]
        public async Task<ActionResult<List<TimeOnly>>> GetAvailabilities(DateOnly date)
        {
            try
            {
                var availabilites = await _eventService.getAvailabilities(date);
                return availabilites;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("/rdvs/{date}")]
        public async Task<ActionResult<Event>> GetEventByDate(DateOnly date)
        {
            try
            {
                var ev = await _eventService.GetEventByDate(date);
                return Ok(ev);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("appointements/status/data")]
        public async Task<ActionResult<AppointementDataByStatus[]>> GetAppointementsDataByStatus()
        {
            try
            {
                var data = _eventService.GetAppointementsDataByStatus();
                return Ok(data);
            }catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("/appointement/stats")]
        public async Task<IActionResult> getLeadsStats()
        {
            try
            {
                var appointementsData = _eventService.GetAppointementsData();
                return Ok(appointementsData);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
