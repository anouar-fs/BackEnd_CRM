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
                var ev = await _eventService.createEvent(evnt);
                return Ok(ev);
            
        }

        [HttpGet]
        public async Task<ActionResult<Event>> GetEventByLead_ConseillerId(int leadId, int ConseillerId)
        {
            var ev = await _eventService.GetEventByConseillerId_leadId(leadId, ConseillerId);
            return Ok(ev);
            
        }

        [HttpGet("/Time/Advisor")]
        public async Task<ActionResult<Event>> GetEventByDate_Hour_ConseillerId(TimeOnly time, int ConseillerId, DateTime date)
        {
                var ev = await _eventService.GetEventByConseillerId_leadId(0, ConseillerId);
                return Ok(ev);
        }

        [HttpGet("/Slot")]
        public async Task<ActionResult<SlotDto>> GetNextSlot()
        {
                var slot = await _eventService.GetNextSlot();

                return Ok(new SlotDto { Date = slot.Date, Slot = slot.Slot });
        }

        [HttpGet("/availabilities/{date}")]
        public async Task<ActionResult<List<TimeOnly>>> GetAvailabilities(DateOnly date)
        {
                var availabilites = await _eventService.getAvailabilities(date);
                return availabilites;
           
        }

        [HttpGet("/rdvs/{date}")]
        public async Task<ActionResult<Event>> GetEventByDate(DateOnly date)
        {
                var ev = await _eventService.GetEventByDate(date);
                return Ok(ev);
        }

        [HttpGet("appointements/status/data")]
        public async Task<ActionResult<AppointementDataByStatus[]>> GetAppointementsDataByStatus()
        {

            var data = _eventService.GetAppointementsDataByStatus();
            return Ok(data);
        }

        [HttpGet("/appointement/stats")]
        public async Task<IActionResult> getLeadsStats()
        {
                var appointementsData = _eventService.GetAppointementsData();
                return Ok(appointementsData);
        }

        [HttpGet("/appointement/Analytics")]
        public async Task<IActionResult> getLeadsAnalytics(int idAdvisor)
        {
                var appointementsAnalytics = _eventService.GetAppointementAnalyticsDataByStatus(idAdvisor);
                return Ok(appointementsAnalytics);
        }
    }
}
