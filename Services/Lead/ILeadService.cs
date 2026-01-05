namespace BackEnd.Services.Lead;
using BackEnd.Entities;
public interface ILeadService
    {
    public Task<Lead> createLead(Lead lead);
    public Task<IEnumerable<Lead>> getLeads();
    public Task<Lead> getLeadById(int id);
    public Task updateAsync(Lead lead);
}

