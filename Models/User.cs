#pragma warning disable CS8618
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petdora.Models;
public class User
{
    [Key]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [MinLength(2, ErrorMessage = "Username must be at least 2 characters")]
    [DisplayName("Username:")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    [UniqueEmail]
    [DisplayName("Email:")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [DisplayName("Password:")]
    public string Password { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "Confirm password is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [DataType(DataType.Password)]
    [DisplayName("Confirm Password:")]
    public string ConfirmPassword { get; set; }

    // nav props
    public List<Pet> OwnedPetsList { get; set; } = new();
    public List<UserPetFollow> FollowedPetsList { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Email is required");
        }

        DataContext _context = (DataContext)validationContext.GetService(typeof(DataContext));
        if(_context.Users.Any(u => u.Email == value.ToString()))
        {
            return new ValidationResult("Email must be unique");
        }
        return ValidationResult.Success;
    }
}