using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Music_Management_System.Models;

public class Song
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Lyrics { get; set; } = string.Empty;

    public string ThumbnailUrl { get; set; } = string.Empty;
    [NotMapped]
    public IFormFile? ThumbnailFile { get; set; }

    public string MP3Url { get; set; } = string.Empty;
    [NotMapped]
    public IFormFile? MP3File { get; set; }

    [Required]
    public DateTime ReleaseDate { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    [Required]
    public int SingerId { get; set; }
    [ForeignKey("SingerId")]
    public Singer? Singer { get; set; }

    [Required]
    public int ComposerId { get; set; }
    [ForeignKey("ComposerId")]
    public Composer? Composer { get; set; }

    public int Status { get; set; } = 1;   // Default to Active
}