#pragma warning disable CS8618
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Petdora.Models;
public class Pet
{
    [Key]
    public int PetId { get; set; }

    public string Name { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string Type { get; set; }

    public string Breed { get; set; }

    public string Bio { get; set; }

    // fk
    public int UserId { get; set; }

    // nav props
    public User? Owner { get; set; }
    public List<UserPetFollow> Followers { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}