#pragma warning disable CS8618
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Petdora.Models;
public class Post
{
    [Key]
    public int PostId { get; set; }
    
    [Required(ErrorMessage = "Image URL is required")]
    [DisplayName("Image (URL format please)")]
    public string ImgURL { get; set; }

    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // fk
    public int PetId { get; set; }

    // nav props
    public Pet? Poster { get; set; }
    public List<UserPostLike> UserLikesList { get; set; } = new();
}