using System.ComponentModel.DataAnnotations;

public class Person
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public required string Name { get; set; }
}
