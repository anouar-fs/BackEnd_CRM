
using BackEnd.Entities;

public class AppointementAnalyticsDataByStatus
{
    public List<AppointementAnalyticsData> appointementAnalyticsDatabyStatus { get; set; }
}

public class AppointementAnalyticsData
{
    public MeetingStatus Id { get; set; }
    public  List<DataAnalytics> Data { get; set; }
}

public class DataAnalytics
{
    public string Month { get; set; }
    public int Total { get; set; }
}

