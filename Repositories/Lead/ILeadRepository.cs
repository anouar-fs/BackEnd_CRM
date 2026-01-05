namespace BackEnd.Repositories.Lead;
using BackEnd.Entities;

public interface ILeadRepository
    {
        public Task<Lead> CreateLead(Lead lead);
        public Task<IEnumerable<Lead>> getLeads();
        public Task<Lead> getLeadById(int id);
        public Task updateAsync(Lead lead);
    }

