using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Music_Management_System.Models;

public class Song
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; } = String.Empty;
    [Required]
    public string Lyrics { get; set; } = String.Empty;
    
    public string ThumbnailUrl { get; set; } = String.Empty;
    [NotMapped]
    public IFormFile? ThumbnailFile { get; set; } = null;
    [Required]
    public string MP3Url { get; set; } = String.Empty;
    [NotMapped]
    public IFormFile? MP3File { get; set; } = null;
    
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

    [Required]
    public int Status { get; set; }
  
  
    

        
    
    
}