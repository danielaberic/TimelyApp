using timely.Modals;

namespace timely
{
    public class ProjectsResponse
    {
        public List<Project> Projects { get; set; }=new List<Project>();
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
