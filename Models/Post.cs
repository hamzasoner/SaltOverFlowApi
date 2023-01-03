using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;

namespace SaltOverFlowApi.Models;

public class Post
{
    [Key] public int PostId { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("UserId")] public int? UserId { get; set; }

    public string Tags { get; set; }

    public byte[]? Image { get; set; }
    [DefaultValue(0)] public int Vote { get; set; }

    public virtual List<Comment> Comments { get; set; }
}