namespace BackEnd.Dto;
public class LeadDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }

    public string LastName { get; set; }
    public string Email { get; set; }

    public string Phone { get; set; }

    public string Source { get; set; }

    public DateTime ReceivedAt { get; set; }
    public string? ProductInterest { get; set; }
    public string? UtmCampaign { get; set; }
    public bool? Welcome_email_sent { get; set; }
}
