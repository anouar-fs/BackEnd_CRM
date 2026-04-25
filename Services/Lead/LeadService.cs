namespace BackEnd.Services.Lead;

using BackEnd.Dto;
using BackEnd.Entities;
using BackEnd.Mapper;
using BackEnd.Repositories.Lead;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mysqlx;
using System.Collections.Generic;

public class LeadService:ILeadService
{
    private ILeadRepository _leadRepository;
    private LeadMapper _mapper;

    public LeadService(ILeadRepository leadRepository,LeadMapper mapper)
    {
        _leadRepository = leadRepository;
        _mapper = mapper;
    }
    public async Task<LeadDto> createLead(LeadDto leadDto) {
        var leadReq = _mapper.ToLead(leadDto);
        var duplicatLead = await _leadRepository.GetLeadByPhoneNumberOrEmail(leadDto.Phone,leadDto.Email);
        if (duplicatLead is not null)
        {
            throw new Exception("Lead with same phone number or email already exists.");
        }
        var leadRep = await _leadRepository.CreateLead(leadReq);
        return _mapper.ToLeadDto(leadRep);
    }

    public async Task<LeadsResponse> getLeads(int page, int pageSize)
    {
        var leads = await _leadRepository.getLeads(page,pageSize);
        var totalItems = _leadRepository.getLeadCount();
        var leadsDto = leads.Select(lead => _mapper.ToLeadDto(lead)).ToList();
        var pagesCount = (int)Math.Ceiling(totalItems / (double)pageSize);

        var result = new LeadsResponse { pageNumber = pagesCount, leads = leadsDto};

        return result;
    }

    public async Task<LeadDto> getLeadById(int id)
    {
        var lead = await _leadRepository.getLeadById(id);
        var leadDto = _mapper.ToLeadDto(lead);
        return leadDto;
    }

    public async Task updateAsync(int id, JsonPatchDocument<Lead> patchDoc)
    {
        var lead = await _leadRepository.getLeadById(id);
        if (lead == null) throw new Exception("Lead not found");

        patchDoc.ApplyTo(lead, new ModelStateDictionary());


        await _leadRepository.updateAsync(lead);
    }

    public async Task<LeadDto> GetLeadByJid(string whatsappJid)
    {
        var lead = await _leadRepository.GetLeadByJid(whatsappJid);
        var leadDto = _mapper.ToLeadDto(lead);
        return leadDto;
    }

    public LeadStats getLeadStats()
    {
        return _leadRepository.getLeadStats();
    }

    public async Task<LeadDto> DeleteLeadByid(int id)
    {
        var lead = await _leadRepository.DeleteLeadByid(id);
        var leadDto = _mapper.ToLeadDto(lead);
        return leadDto;
    }
}

