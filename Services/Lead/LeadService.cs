namespace BackEnd.Services.Lead;
using BackEnd.Entities;
using BackEnd.Repositories.Lead;

public class LeadService:ILeadService
{
    private ILeadRepository _leadRepository;

    public LeadService(ILeadRepository leadRepository)
    {
        _leadRepository = leadRepository;
    }
    public async Task<Lead> createLead(Lead lead) {
        return await _leadRepository.CreateLead(lead);

    }

    public async Task<IEnumerable<Lead>> getLeads()
    {
        var leads = await _leadRepository.getLeads();
        return leads;
    }

    public async Task<Lead> getLeadById(int id)
    {
        var lead = await _leadRepository.getLeadById(id);
        return lead;
    }

    public async Task updateAsync(Lead lead)
    {
        _leadRepository.updateAsync(lead);
    }
}

