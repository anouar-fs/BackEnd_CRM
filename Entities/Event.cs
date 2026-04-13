namespace BackEnd.Entities;
public class Event
{
    public int Id { get; set; }
    public User? advisor { get; set; }
    public Lead? Lead { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly HeureDebut { get; set; }
    public MeetingStatus Statut { get; set; }
    public DateTime CreatedAt { get; set; }
}

public enum MeetingStatus
{
    Planned = 0,
    Canceled = 1,
    presented = 2,
    absent = 3
}