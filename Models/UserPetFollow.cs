#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

namespace Petdora.Models;
public class UserPetFollow
{
    [Key]
    public int UserPetFollowId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // fk
    public int UserId { get; set; }
    public int PetId { get; set; }

    // nav props
    public User? UserThatFollowed { get; set; }
    public Pet? FollowedPet { get; set; }
}