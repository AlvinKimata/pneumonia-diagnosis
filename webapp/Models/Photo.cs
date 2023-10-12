public class Photo
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string PhotoPath { get; set; }
}
