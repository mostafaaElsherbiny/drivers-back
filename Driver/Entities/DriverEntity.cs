
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Driver.Entities;

public class DriverEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, MaxLength(50), MinLength(3)]
    public string FirstName { get; set; } = null!;

    [Required, MaxLength(50), MinLength(3)]
    public string LastName { get; set; } = null!;

    [Required, MaxLength(50), MinLength(3), EmailAddress]
    public string Email { get; set; } = null!;

    [Required, MaxLength(50), MinLength(3)]
    public string PhoneNumber { get; set; } = null!;

}
