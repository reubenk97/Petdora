#pragma warning disable CS8618
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Petdora.Models;
public class LogUser
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    [DisplayName("Email:")]
    public string LogEmail { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [DisplayName("Password:")]
    public string LogPassword { get; set; }
}