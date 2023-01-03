using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaltOverFlowApi.Models;

public class Comment
{
    [Key] public int CommentId { get; set; }

    public string Text { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("UserId")] public int? UserId { get; set; }

    [ForeignKey("PostId")] public int? PostId { get; set; }
}