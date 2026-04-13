namespace BackEnd.Repositories.Event;
using BackEnd.Entities;

public interface IEventRepository
{
    public Task<Event> CreateEvent(Event evnt);
    public Task<Event> GetEventByConseillerId_leadId(int conseillerId,int leadId);

    public Task<(DateOnly Date, TimeOnly Slot)> GetNextSlot();
    public Task<List<TimeOnly>> getAvailabilities(DateOnly date);
    public Task<Boolean> checkTimeSlotBusy(Event evnt);
    public Task<int> getFreeAdvisorId();
    public Task<IEnumerable<Event>> GetEventByDate(DateOnly date);
    public IEnumerable<AppointementDataByStatus> GetAppointementsDataByStatus();
    public AppointementData GetAppointementsData();
}

