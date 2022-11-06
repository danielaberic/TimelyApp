using System.ComponentModel.DataAnnotations;

namespace timely.Modals
{
    public class Time
    { 
        public int Id { get; set; } 
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? Total { get; set; }

        //navigation
        public int ProjectId { get; set; }
    }
}
