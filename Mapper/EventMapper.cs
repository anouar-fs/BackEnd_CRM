
using BackEnd.Entities;

public class EventMapper
{
    public Event ToEvent(EventDto eventDto)
    {
        return new Event
        {
            Date = eventDto.Date,
            CreatedAt = eventDto.CreatedAt,
            HeureDebut = eventDto.HeureDebut,
            Statut = eventDto.Statut,
        };
        }
}

