using System.ComponentModel.DataAnnotations;

namespace MyApp.DTOs;

public class AddAnimal
{
    [Required]
    public int IdAnimal { get; set; }
    [Required]
    [MaxLength(200)]
    [MinLength(1)]
    public string Name { get; set; }
    [MaxLength(200)]
    [MinLength(1)]
    public string? Description  { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Category  { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Area  { get; set; }
}