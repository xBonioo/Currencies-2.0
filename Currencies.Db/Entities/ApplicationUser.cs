using Microsoft.AspNetCore.Identity;

namespace Currencies.Db.Entities;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
    public bool IsActive { get; set; } = true;
    public int? RoleId { get; set; }

    public string? Address { get; set; }
    public int IdentityNumber { get; set; }
    public string? IdNumber { get; set; }
    public DateTime IdExpiryDate { get; set; }
    public DateTime IdIssueDate { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public virtual Role Role { get; set; }
    public virtual ICollection<UserCurrencyAmount> UserCurrencyAmounts { get; set; }
    public virtual ICollection<UserExchangeHistory> UserExchangeHistory { get; set; }
}

public class TokenUser : IdentityUserToken<string>
{
    public DateTime ValidUntil { get; set; }
    public bool IsActive => DateTime.UtcNow <= ValidUntil;
}