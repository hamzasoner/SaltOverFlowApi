using System.ComponentModel.DataAnnotations;

namespace SaltOverFlowApi.Models;

public class User
{
    [Key] public int UserId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public virtual List<Post> Posts { get; set; }

    public virtual List<Comment> Comments { get; set; }
}