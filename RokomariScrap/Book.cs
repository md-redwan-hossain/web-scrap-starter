using System.ComponentModel.DataAnnotations;

namespace RokomariScrap;

public class Book
{
    public int Id { get; set; }
    [MaxLength(1000)] public required string Title { get; set; }
    [MaxLength(1000)] public required string Author { get; set; }
}