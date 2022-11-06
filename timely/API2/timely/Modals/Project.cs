using System.ComponentModel.DataAnnotations;

namespace timely.Modals
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string StartDate { get; set; }
        public string? EndDate { get; set; }
        public TimeSpan? Total { get; set; }

        public ICollection<Time> Times { get; set; }
    }
}
