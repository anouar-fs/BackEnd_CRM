namespace BackEnd.Repositories.Lead;
using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;

public class LeadRepository:ILeadRepository
{
    private AppDbContext _dbContext;

    public LeadRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Lead> CreateLead(Lead lead)
    {
        await _dbContext.Leads.AddAsync(lead);
        await _dbContext.SaveChangesAsync();
        return lead;
    }

    public async Task<Lead> getLeadById(int id)
    {
        var lead = await _dbContext.Leads.FirstAsync(l => l.Id == id);

        return lead;
    }

    public async Task<IEnumerable<Lead>> getLeads(int page, int pageSize)
    {
        var leads = await _dbContext.Leads.OrderByDescending(x => x.ReceivedAt)
                                          .Skip(pageSize * (page - 1))
                                          .Take(pageSize)
                                          .ToListAsync();
        return leads;
    }

    public async Task updateAsync(Lead lead)
    {
        try
        {
            _dbContext.Leads.Update(lead);
            var results = await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString()); // put breakpoint here

            throw; // optional: rethrow
        }
    }

    public int getLeadCount()
    {
        return _dbContext.Leads.Count();
    }

    public LeadStats getLeadStats()
    {
        var hotLeadscount = _dbContext.Leads.Count(l => l.WhatsappAnswer == true);
        var coldLeadscount = _dbContext.Leads.Count(l => l.WhatsappAnswer == false);
        return new LeadStats { coldLeads = hotLeadscount, hotLeads = coldLeadscount };
    }

    public Task<Lead> GetLeadByJid(string whatsappJid)
    {
        var lead = _dbContext.Leads.FirstAsync(l => l.WhatsappJid == whatsappJid);

        return lead;
    }

    public async Task<Lead> GetLeadByPhoneNumberOrEmail(string phone,string email)
    {
        var lead = await _dbContext.Leads.FirstOrDefaultAsync(l => l.Phone == phone || l.Email == email);

        return lead;
    }

    public async Task<Lead> DeleteLeadByid(int id)
    {
        var lead = await getLeadById(id);

        if (lead == null)
            return null;

        _dbContext.Leads.Remove(lead);
        await _dbContext.SaveChangesAsync();

        return lead;
    }
}

