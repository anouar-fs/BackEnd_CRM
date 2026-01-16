namespace BackEnd.Services.Lead;

using BackEnd.Dto;
using BackEnd.Entities;
public interface ILeadService
    {
    public Task<LeadDto> createLead(LeadDto lead);
    public Task<LeadsResponse> getLeads(int page, int pageSize);
    public Task<LeadDto> getLeadById(int id);
    public Task updateAsync(LeadDto lead);
}

