using System.ComponentModel.DataAnnotations;

namespace timely.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? Total { get; set; }
        
    }
}
