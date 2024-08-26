#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

namespace Petdora.Models;
public class UserPostLike
{
    [Key]
    public int UserPostLikeId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // fk
    public int UserId { get; set; }
    public int PostId { get; set; }

    // nav props
    public User? UserThatLiked { get; set; }
    public Post? LikedPost { get; set; }
}