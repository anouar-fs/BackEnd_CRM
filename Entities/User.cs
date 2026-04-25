namespace BackEnd.Entities;
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string password { get; set; } = string.Empty;
    public UserRole Role { get; set; }

}

public enum UserRole
{
    Advisor = 1,
    Admin = 2,
    CRMManager = 3
}