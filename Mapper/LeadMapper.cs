namespace BackEnd.Mapper;

using BackEnd.Dto;
using BackEnd.Entities;
public class LeadMapper
{
    public Lead ToLead(LeadDto leadDto) 
    {
        return new Lead { 

            FirstName = leadDto.FirstName,
            LastName = leadDto.LastName,
            Email = leadDto.Email,
            Phone = leadDto.Phone,
            Source = leadDto.Source,
            ReceivedAt = leadDto.ReceivedAt,
            ProductInterest = leadDto.ProductInterest,
            UtmCampaign = leadDto.UtmCampaign,
            Welcome_email_sent = leadDto.Welcome_email_sent,
            WhatsappJid = leadDto.whatsappJid,
            WhatsappAnswer = leadDto.WhatsappAnswer
        };
    }

    public LeadDto ToLeadDto(Lead lead)
    {
        return new LeadDto
        {
            Id = lead.Id,
            FirstName = lead.FirstName,
            LastName = lead.LastName,
            Email = lead.Email,
            Phone = lead.Phone,
            Source = lead.Source,
            ReceivedAt = lead.ReceivedAt,
            ProductInterest = lead.ProductInterest,
            UtmCampaign = lead.UtmCampaign,
            Welcome_email_sent = lead.Welcome_email_sent,
            whatsappJid = lead.WhatsappJid,
            WhatsappAnswer = lead.WhatsappAnswer
        };
    }
}

