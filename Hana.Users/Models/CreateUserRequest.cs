using System.ComponentModel.DataAnnotations;

using Hana.Users.Entities;

namespace Hana.Users.Models;

public class CreateUserRequest
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    [Required]
    [EnumDataType(typeof(Role))]
    public string? Role { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [MinLength(6)]
    public string? Password { get; set; }

    [Required]
    [Compare("Password")]
    public string? ConfirmPassword { get; set; }
}
