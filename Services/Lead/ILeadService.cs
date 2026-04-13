namespace BackEnd.Services.Lead;

using BackEnd.Dto;
using BackEnd.Entities;
using Microsoft.AspNetCore.JsonPatch;

public interface ILeadService
    {
    public Task<LeadDto> createLead(LeadDto lead);
    public Task<LeadsResponse> getLeads(int page, int pageSize);
    public Task<LeadDto> getLeadById(int id);
    public Task<LeadDto> GetLeadByJid(string whatsappJid);
    public Task updateAsync(int id, JsonPatchDocument<Lead> patchDoc);
    public LeadStats getLeadStats();

}

