
namespace BackEnd.Repositories.Event;
using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;

public class EventRepository : IEventRepository
{
    private AppDbContext _context;


    public EventRepository(AppDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<int> getFreeAdvisorId()
    {
        var advisors = await _context.Users.Select(u => u.Id).ToListAsync();
        var selectedAdvisorId = advisors.Select(id => new
        {
            Id = id,
            Count = _context.Events.Count(e =>
            e.advisor.Id == id &&
            e.Statut == MeetingStatus.Planned)
        })
        .OrderBy(x => x.Count)
        .Select(x => x.Id)
        .FirstOrDefault();
        return selectedAdvisorId;
    }
    public async Task<Event> CreateEvent(Event evnt)
    {
        var selectedAdvisorId = await getFreeAdvisorId();
        evnt.advisor.Id = selectedAdvisorId;

        await _context.Events.AddAsync(evnt);
        await _context.SaveChangesAsync();
        return evnt;
    }

    public async Task<(DateOnly Date, TimeOnly Slot)> GetNextSlot()
    {
        var selectedAdvisorId = await getFreeAdvisorId();

        var startDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

        var slots = new List<TimeOnly>
    {
        new TimeOnly(9, 0),
        new TimeOnly(10, 0),
        new TimeOnly(11, 0),
        new TimeOnly(14, 0),
        new TimeOnly(15, 0),
        new TimeOnly(16, 0),
        new TimeOnly(17, 0),
    };

        for (int i = 0; i < 30; i++)
        {
            var date = startDate.AddDays(i);

            foreach (var slot in slots)
            {
                bool isBusy = await _context.Events.AnyAsync(e =>
                    e.advisor.Id == selectedAdvisorId &&
                    e.Date == date &&
                    e.HeureDebut == slot &&
                    e.Statut == MeetingStatus.Planned);

                if (!isBusy)
                {
                    return (date, slot);
                }
            }
        }

        throw new Exception("No available slot found in the next 30 days.");
    }


    public async Task<Event> GetEventByConseillerId_leadId(int conseillerId, int leadId)
    {
    return  await _context.Events
        .Include(e => e.Lead)
        .Include(e => e.advisor)
        .Where(e => e.Lead != null && e.advisor != null)
        .FirstOrDefaultAsync(e =>
            e.Lead.Id == leadId && e.advisor.Id == conseillerId
        );
    }

    public async Task<Boolean> checkTimeSlotBusy(Event evnt)
    {
        var advisorId = evnt.advisor?.Id;

        if (advisorId == null)
            throw new Exception("Advisor is required");

        var isBusy = await _context.Events
            .AnyAsync(e =>
                e.Date == evnt.Date &&
                e.HeureDebut == evnt.HeureDebut &&
                e.advisor != null &&
                e.advisor.Id == advisorId &&
                e.Statut == MeetingStatus.Planned
            );
        return isBusy;
    }
    public async Task<List<TimeOnly>> getAvailabilities(DateOnly date)
    {
        var slots = new List<TimeOnly>
        {
            new TimeOnly(9, 0),
            new TimeOnly(10, 0),
            new TimeOnly(11, 0),
            new TimeOnly(14, 0),
            new TimeOnly(15, 0),
            new TimeOnly(16, 0),
            new TimeOnly(17, 0),
        };
        var availabilities = new List<TimeOnly>();

        foreach (var slot in slots)
        {
            var selectedAdvisorId = await getFreeAdvisorId();

            bool isBusy = await _context.Events.AnyAsync(e =>
                e.advisor.Id == selectedAdvisorId &&
                e.Date == date &&
                e.HeureDebut == slot &&
                e.Statut == MeetingStatus.Planned);

            if (!isBusy)
            {
                availabilities.Add(slot);
            }
        }
        return availabilities;
    }

    public async Task<IEnumerable<Event>> GetEventByDate(DateOnly date)
    {
       var rdvs = await _context.Events.Include(e => e.Lead)
        .Include(e => e.advisor)
        .Where(e => e.Lead != null && e.advisor != null)
        .Where(e => e.Date == date).OrderByDescending(e => e.HeureDebut).ToListAsync();
        return rdvs;
    }

    public IEnumerable<AppointementDataByStatus> GetAppointementsDataByStatus()
    {
        var status = new[] { MeetingStatus.presented, MeetingStatus.Canceled, MeetingStatus.Planned, MeetingStatus.absent };
        var data = new List<AppointementDataByStatus> { };
        var totalAll = _context.Events.Count();
        
        foreach (var statu in status)
        {
            var total = _context.Events.Count(e => e.Statut == statu);
            var percentage = Math.Round(((decimal)total / totalAll) * 100, 2);
            data.Add(new AppointementDataByStatus { MeetingStatus = statu,total = total, percentage = (double)percentage });
        }

        return data;
    }

    public AppointementData GetAppointementsData()
    {
        var now = DateTime.Now;
        var beginOfMonth = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1);
        var endDate = beginOfMonth.AddMonths(1).AddDays(-1);

        var MonthlyAppoints = _context.Events.Count(e => e.Date >= beginOfMonth && e.Date <= endDate);
        var Monthlylds = _context.Leads.Where(l => DateOnly.FromDateTime(l.ReceivedAt) >= beginOfMonth && DateOnly.FromDateTime(l.ReceivedAt) <= endDate).Count();
        var MonthlyAdvrs = _context.Events.Where(e => e.Date >= beginOfMonth && e.Date <= endDate).Select(e => e.advisor.Id).Distinct().Count();

        return new AppointementData { MonthlyAdvisors = MonthlyAdvrs, MonthlyAppointements = MonthlyAppoints, MonthlyLeads = Monthlylds };

    }

    public Dictionary<MeetingStatus, List<DataAnalytics>> GetAppointementAnalyticsDataByStatus(int idAdvisor)
    {
        var currentMonth = DateTime.Now.Month;
        var i = 1;

        var statsBystatus = new List<DataAnalytics>();

        var AppointementAnalyticsDataByPlanned = new AppointementAnalyticsData { Id = MeetingStatus.Planned, Data = { } };
        var AppointementAnalyticsDataByAbsent = new AppointementAnalyticsData { Id = MeetingStatus.absent, Data = { } };
        var AppointementAnalyticsDataByCanceled = new AppointementAnalyticsData { Id = MeetingStatus.Canceled, Data = { } };
        var AppointementAnalyticsDataByPresented = new AppointementAnalyticsData { Id = MeetingStatus.presented, Data = { } };

        Dictionary<MeetingStatus, List<DataAnalytics>> dict = new Dictionary<MeetingStatus, List<DataAnalytics>>();
        dict[MeetingStatus.Planned] = new List<DataAnalytics>();
        dict[MeetingStatus.Canceled] = new List<DataAnalytics>();
        dict[MeetingStatus.presented] = new List<DataAnalytics>();
        dict[MeetingStatus.absent] = new List<DataAnalytics>();

        var statuses = new[]
            {
                MeetingStatus.Planned,
                MeetingStatus.absent,
                MeetingStatus.Canceled,
                MeetingStatus.presented
            };


        while (i <= currentMonth)
        {
            var beginOfMonth = new DateOnly(DateTime.Now.Year, i, 1);
            var endDate = beginOfMonth.AddMonths(1).AddDays(-1);

            foreach (var status in statuses)
            {
                dict[status].Add(new DataAnalytics
                {
                    Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMM", new CultureInfo("en-US")),
                    Total = _context.Events.Count(e => e.Date >= beginOfMonth && e.Date <= endDate && e.Statut == status && e.advisor.Id == idAdvisor)
                });
            }
            i++;
        }
        return dict;
    }

}

