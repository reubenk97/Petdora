using Microsoft.AspNetCore.Mvc;
using Petdora.Models;
using Microsoft.EntityFrameworkCore;

namespace Petdora.Controllers;

[SessionCheck]
[Route("posts")]
public class PostController : Controller
{
    private readonly ILogger<PostController> _logger;
    private DataContext _context;

    public PostController(ILogger<PostController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("")]
    public ViewResult Dashboard()
    {
        List<Post> PostsList = _context.Posts
                                        .Include(p => p.Poster)
                                        .Include(p => p.UserLikesList)
                                        .ThenInclude(upl => upl.UserThatLiked)
                                        .OrderByDescending(p => p.CreatedAt)
                                        .Take(3)
                                        .ToList();
        return View(PostsList);
    }

    [HttpGet("new")]
    public ViewResult NewPost()
    {
        return View();
    }

    [HttpPost("create")]
    public IActionResult CreatePost(Post newPost)
    {
        if (!ModelState.IsValid)
        {
            var message = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            Console.WriteLine(message);
            return View("NewPost");
        }
        newPost.PetId = (int)HttpContext.Session.GetInt32("PetId");
        _context.Add(newPost);
        _context.SaveChanges();
        return RedirectToAction("ViewPost", new{postId = newPost.PostId});
    }

    [HttpGet("{postId}")]
    public IActionResult ViewPost(int postId)
    {
        Post? ThisPost = _context.Posts
                                    .Include(p => p.Poster)
                                    .Include(p => p.UserLikesList)
                                    .FirstOrDefault(p => p.PostId == postId);
        if(ThisPost == null)
        {
            return RedirectToAction("Dashboard");
        }
        return View(ThisPost);
    }

    [HttpGet("{postId}/edit")]
    public IActionResult EditPost(int postId)
    {
        Post? thisPost = _context.Posts.FirstOrDefault(p => p.PostId == postId);
        if (thisPost == null)
        {
            return RedirectToAction("Dashboard");
        }
        return View(thisPost);
    }

    [HttpPost("{postId}/update")]
    public IActionResult UpdatePost(int postId, Post editedPost)
    {
        Post? oldPost = _context.Posts.FirstOrDefault(p => p.PostId == postId);
        if (oldPost == null)
        {
            return RedirectToAction("Dashboard");
        }
        if (!ModelState.IsValid)
        {
            var message = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            Console.WriteLine(message);
            return View("EditPost", editedPost);
        }
        oldPost.Title = editedPost.Title;
        oldPost.UpdatedAt = DateTime.Now;
        _context.SaveChanges();
        return RedirectToAction("ViewPost", new{postId});
    }

    [HttpPost("{postId}/delete")]
    public IActionResult DeletePost(int postId)
    {
        Post? DeletedPost = _context.Posts.SingleOrDefault(p => p.PostId == postId);
        if (DeletedPost != null)
        {
            _context.Remove(DeletedPost);
            _context.SaveChanges();
        }
        return RedirectToAction("Dashboard");
    }

    [HttpPost("{postId}/like")]
    public IActionResult ToggleLike(int postId)
    {
        int UserId = (int)HttpContext.Session.GetInt32("UserId");
        UserPostLike? ExistingLike = _context.UserPostLikes.FirstOrDefault(upl => upl.UserId == UserId && upl.PostId == postId);
        if (ExistingLike == null)
        {
            UserPostLike NewLike = new(){UserId = UserId, PostId = postId};
            _context.Add(NewLike);
        }
        else
        {
            _context.Remove(ExistingLike);
        }
        _context.SaveChanges();
        return RedirectToAction("Dashboard");
    }
}