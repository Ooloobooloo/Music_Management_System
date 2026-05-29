using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Music_Management_System.Models;

public class Composer
{
 
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }

    [Required] 
    public string Biography { get; set; } = string.Empty;
   
    [Required]
    public string ImageUrl { get; set; } = string.Empty;
   
    [NotMapped]
    public IFormFile ImageFile { get; set; }  = null;


}
