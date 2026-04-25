namespace BackEnd.Repositories.Lead;
using BackEnd.Entities;

public interface ILeadRepository
    {
        public Task<Lead> CreateLead(Lead lead);
        public Task<IEnumerable<Lead>> getLeads(int page, int pageSize);
        public Task<Lead> getLeadById(int id);
        public Task<Lead> GetLeadByJid(string whatsappJid);
        public Task updateAsync(Lead lead);
        public int getLeadCount();
        public LeadStats getLeadStats();
        public Task<Lead> GetLeadByPhoneNumberOrEmail(string phone, string email);
        public Task<Lead> DeleteLeadByid(int id);
    }

