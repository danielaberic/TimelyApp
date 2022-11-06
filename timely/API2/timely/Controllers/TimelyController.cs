using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using timely.Data;
using timely.Helpers;
using timely.Modals;

namespace timely.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimelyController : Controller
    {
        private readonly TimelyDbContext _context;

        public TimelyController(TimelyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginatedProjects([FromQuery] PaginationParameters @params)
        {
            if (_context.Timely == null)
                return NotFound();

            var projects = _context.Timely
            .OrderBy(x => x.Id);

            Pagination.AddPagination(this.Response,@params, projects.Count());
            var items = await projects
                .Skip((@params.Page - 1) * @params.ItemsPerPage)
                .Take(@params.ItemsPerPage)
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("AllProjects")]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _context.Timely.ToListAsync();

            return Ok(projects);
        }

        //[HttpGet]
        ////[Route("{page:int}")]
        //public async Task<IActionResult> GetAllProjects(int pageNumber = 1, int pageSize = 10)
        //{
        //    if (_context.Timely == null)
        //        return NotFound();
        //    pageNumber = (pageNumber <= 0) ? 1 : pageNumber;
        //    pageSize = (pageSize <= 0) ? 10 : pageSize;

        //    //var pageResults = 3;
        //    var pageCount = (int)Math.Ceiling(_context.Timely.Count() / (double)pageSize);

        //    var projects = await _context.Timely
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync();
        //    var response = new ProjectsResponse
        //    {
        //        Projects = projects,
        //        CurrentPage = pageNumber,
        //        TotalPages = pageCount
        //    };

        //    return Ok(response);
        //}

        [HttpGet]
        [Route("{id:int}")]
        [ActionName("GetProject")]
        public async Task<IActionResult> GetProject([FromRoute] int id)
        {
            //var project = await _context.Timely.FirstOrDefaultAsync(x => x.Id == id);
            var project = from proj in _context.Timely
                          where proj.Id == id
                          select proj;
            var times = (from t in _context.Times
                          where t.ProjectId == id
                          select new
                          {
                              Id = t.Id,
                              StartDate = t.StartDate,
                              EndDate = t.EndDate,
                              ProjectId=t.ProjectId
                          }).ToList();
            dynamic result = new System.Dynamic.ExpandoObject();
            result.project = project;
            result.times = times;   
            if (project != null) { return Ok(result); }
            else { return NotFound("Project Not Found!"); }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateProject([FromRoute] int id, [FromBody] Project project)
        {
            var existingProject = from proj in _context.Timely
                                  where proj.Id == id
                                  select proj;

            try
            {
                if (existingProject.SingleOrDefault() == null)
                    return NotFound();

                Project p=existingProject.SingleOrDefault();

                    p.Name = project.Name;

                try
                {
                    await _context.SaveChangesAsync();
                    return Ok(existingProject);
                }
                catch (Exception e)
                {
                    return NotFound("Project not found!");
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }  
           
        }
        //DeletingAProject
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteProject([FromRoute] int id)
        {
            var existingProject = from proj in _context.Timely
                                  where proj.Id == id
                                  select proj;

            try
            {
                if (existingProject.SingleOrDefault() == null)
                    return NotFound("Project not found!");

                 _context.Timely.RemoveRange(_context.Timely.Where(x => x.Id == id));
                 _context.Times.RemoveRange(_context.Times.Where(x => x.ProjectId == id));
                 await _context.SaveChangesAsync();
                    return Ok(existingProject);               
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }            
        }
        [HttpGet("AddSession/{id:int}")]

        public async Task<IActionResult> AddSession([FromRoute] int id)
        {
            var existingProject = (from proj in _context.Timely
                                   where proj.Id == id
                                   select proj
                                 );
            try
            {
                if (existingProject.SingleOrDefault() == null) 
                    return NotFound(); 

                Time time = new Time
                {
                StartDate = DateTime.Now,
                EndDate = null,
                Total = TimeSpan.Zero,
                ProjectId = id
                };
                await _context.Times.AddAsync(time);
                try
                {
                    await _context.SaveChangesAsync();
                    return Ok(existingProject);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }            
        }

        [HttpGet("EndSession/{id:int}")]

        public async Task<IActionResult> EndSession([FromRoute] int id)
        {
            var existingProject = (from proj in _context.Timely
                                   where proj.Id == id
                                   select proj
                                 );
            if (existingProject == null) { return NotFound(); }
            try
            {
                if(existingProject.SingleOrDefault()==null)
                    return NotFound();

            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            Project p = existingProject.SingleOrDefault();
            var existingTime = (from t in _context.Times
                                where t.ProjectId == id
                                select t).ToList();

            foreach (Time t in existingTime)
            {
                if (t.EndDate == null)
                {
                    t.EndDate = DateTime.Now;
                    t.Total = DateTime.Now - t.StartDate;
                    
                    p.EndDate = DateTime.Now.ToString("MM.dd.yyyy HH:mm");
                    p.Total = p.Total + (DateTime.Now - t.StartDate);                                      
                }                
            }            
            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingProject);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpGet("CreateSession")]
        public async Task<IActionResult> AddProject()
        {
            Project proj = new Project
            {
                Name = null,
                StartDate = DateTime.Now.ToString("MM.dd.yyyy HH:mm"),
                EndDate = null,
                Total = TimeSpan.Zero
            };
            try
            {
                await _context.Timely.AddAsync(proj);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
           
            Time time = new Time
            {
                StartDate = DateTime.Now,
                EndDate = null,
                Total = null,
                ProjectId = proj.Id
            };
            try
            {
                await _context.Times.AddAsync(time);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }          

            return CreatedAtAction(nameof(GetProject), new { id = proj.Id }, proj);
        }
    }
}
