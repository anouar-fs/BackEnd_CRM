namespace BackEnd.Services.Lead;

using BackEnd.Dto;
using BackEnd.Entities;
using BackEnd.Exceptions;
using BackEnd.Mapper;
using BackEnd.Repositories.Lead;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Mysqlx;
using System.Collections.Generic;

public class LeadService:ILeadService
{
    private ILeadRepository _leadRepository;
    private TypesenseService _typesenseService;
    private LeadMapper _mapper;

    public LeadService(ILeadRepository leadRepository,LeadMapper mapper,TypesenseService typesenseService)
    {
        _leadRepository = leadRepository;
        _mapper = mapper;
        _typesenseService = typesenseService;
    }
    public async Task<LeadDto> createLead(LeadDto leadDto) {
        var leadReq = _mapper.ToLead(leadDto);
        var duplicatLead = await _leadRepository.GetLeadByPhoneNumberOrEmail(leadDto.Phone,leadDto.Email);
        if (duplicatLead is not null)
        {
            throw new ConflictException("Lead with same phone number or email already exists.");
        }
        var leadRep = await _leadRepository.CreateLead(leadReq);
        var leadIndex = _mapper.ToLeadIndexDto(leadReq);
        await _typesenseService.IndexLeadAsync(leadIndex);
        return _mapper.ToLeadDto(leadRep);
    }

    public async Task createCollections()
    {

        var leads = await _leadRepository.GetAllLeads();
        foreach (var lead in leads) 
        {
            var leadDto = _mapper.ToLeadDto(lead); 
            var leadIndex = _mapper.ToLeadIndexDto(lead);
            await _typesenseService.IndexLeadAsync(leadIndex);
        }
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
        if (lead == null) throw new NotFoundException("Lead not found");

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

