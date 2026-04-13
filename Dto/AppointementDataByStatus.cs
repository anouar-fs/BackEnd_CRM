
using BackEnd.Entities;

public class AppointementDataByStatus
{
    public MeetingStatus MeetingStatus { get; set; }
    public int total { get; set; }
    public double percentage { get; set; }
}

public class AppointementData
{
    public int MonthlyAppointements;
    public int MonthlyLeads;
    public int MonthlyAdvisors;
}