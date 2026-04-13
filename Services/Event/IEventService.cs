namespace BackEnd.Services.Event;
using BackEnd.Entities;
public interface IEventService
    {
        public Task<Event> createEvent(EventDto lead);
        public Task<Event> GetEventByConseillerId_leadId(int conseillerId, int leadId);
        public Task<(DateOnly Date, TimeOnly Slot)> GetNextSlot();
        public Task<List<TimeOnly>> getAvailabilities(DateOnly date);
        public Task<IEnumerable<Event>> GetEventByDate(DateOnly date);
        public IEnumerable<AppointementDataByStatus> GetAppointementsDataByStatus();
        public AppointementData GetAppointementsData();
}

