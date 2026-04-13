namespace BackEnd.Services.Event;
using BackEnd.Entities;
using BackEnd.Repositories;
using BackEnd.Repositories.Event;
using BackEnd.Repositories.Lead;
using System.Threading.Tasks;

public class EventService : IEventService
{
    private IEventRepository _eventRepository;
    private ILeadRepository _leadRepository;
    private IUserRepository _userRepository;
    private EventMapper _eventMapper;

    public EventService(IEventRepository eventRepository, ILeadRepository leadRepository, IUserRepository userRepository, EventMapper eventMapper)
    {
        _eventRepository = eventRepository;
        _leadRepository = leadRepository;
        _eventMapper = eventMapper;
        _userRepository = userRepository;
    }
    public async Task<Event> createEvent(EventDto evntDto)
    {
        var vnt = _eventMapper.ToEvent(evntDto);
        var freeAdvisor = await _eventRepository.getFreeAdvisorId();
        var ld = await _leadRepository.getLeadById(evntDto.Lead);
        var advisor = await _userRepository.getUserById(freeAdvisor);
        vnt.Lead = ld;
        vnt.advisor = advisor;

        var isBusy = await _eventRepository.checkTimeSlotBusy(vnt);
        if (isBusy)
        {
            throw new InvalidOperationException("The selected time slot is already taken.");
        }
        var eventRep = await _eventRepository.CreateEvent(vnt);
        return eventRep;
    }

    public async Task<Event> GetEventByConseillerId_leadId(int conseillerId, int leadId)
    {
        var eventRep = await _eventRepository.GetEventByConseillerId_leadId(conseillerId, leadId);
        return eventRep;
    }

    public async Task<(DateOnly Date, TimeOnly Slot)> GetNextSlot()
    { 
        return await _eventRepository.GetNextSlot();
    }
    public Task<List<TimeOnly>> getAvailabilities(DateOnly date)
    {
        var availabilities = _eventRepository.getAvailabilities(date);
        return availabilities;
    }

    public async Task<IEnumerable<Event>> GetEventByDate(DateOnly date)
    {
        var rdvs = await _eventRepository.GetEventByDate(date);
        return rdvs;
    }
    public IEnumerable<AppointementDataByStatus> GetAppointementsDataByStatus()
    {
        var data = _eventRepository.GetAppointementsDataByStatus();
        return data;
    }

    public AppointementData GetAppointementsData()
    {
        var data = _eventRepository.GetAppointementsData();
        return data;
    }
}
