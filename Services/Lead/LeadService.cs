namespace BackEnd.Services.Lead;

using BackEnd.Dto;
using BackEnd.Entities;
using BackEnd.Mapper;
using BackEnd.Repositories.Lead;

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

    public async Task updateAsync(LeadDto leadDto)
    {
        var lead = _mapper.ToLead(leadDto);
        _leadRepository.updateAsync(lead);
    }
}

