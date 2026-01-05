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

    public async Task<IEnumerable<Lead>> getLeads()
    {
        var leads = await _dbContext.Leads.ToListAsync();

        return leads;
    }

    public async Task updateAsync(Lead lead)
    {
        _dbContext.Leads.Update(lead);
        await _dbContext.SaveChangesAsync();
    }
}

