
using BackEnd.Entities;

public class EventDto
{
    public int Id { get; set; }
    public int advisor { get; set; }
    public int Lead { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly HeureDebut { get; set; }
    public MeetingStatus Statut { get; set; }
    public DateTime CreatedAt { get; set; }
}
