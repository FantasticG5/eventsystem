
using System.ComponentModel.DataAnnotations;


namespace Data.Entities;

public class TrainingClassEntity
{
    [Key] // Primary key
    public int Id { get; set; }

    [Required] // Title is required
    [MaxLength(200)] // Limit title length
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)] // Optional description
    public string Description { get; set; } = string.Empty;

    [Required] // Date and time is required
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required] // Place is required
    [MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    [Required] // Instructor is required
    [MaxLength(150)]
    public string Instructor { get; set; } = string.Empty;

    [Range(1, 500)] // Max participants should be a sensible number
    public int Capacity { get; set; }

    // Optional: How many are booked right now
    public int ReservedSeats { get; set; }

    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

}