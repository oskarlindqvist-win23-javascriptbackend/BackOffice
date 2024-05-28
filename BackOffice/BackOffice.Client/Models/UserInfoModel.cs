using System.ComponentModel.DataAnnotations;

namespace BackOffice.Client.Models;

public class UserInfoModel
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Biography { get; set; }
    public string? ProfileImage { get; set; }
    public string? PhoneNumber { get; set; }
    public AddressModel? Address { get; set; }
}

public class AddressModel
{
    public string AddressLine_1 { get; set; } = null!;
    public string? AddressLine_2 { get; set; }
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
}
