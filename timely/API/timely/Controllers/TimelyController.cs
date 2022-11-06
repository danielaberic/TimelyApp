using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using timely.Data;
using timely.Models;

namespace timely.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimelyController:Controller
    {
        private readonly TimelyDbContext _context;

        public TimelyController(TimelyDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _context.Timely.ToListAsync();

            return Ok(projects);
        }

        [HttpGet]
        [Route("{id:int}")]
        [ActionName("GetProject")]
        public async Task<IActionResult> GetProject([FromRoute] int id)
        {
            //var project = await _context.Timely.FirstOrDefaultAsync(x => x.Id == id);
            var project = from proj in _context.Timely
                          where proj.Id == id
                          select proj;

            if (project != null) { return Ok(project); }
            else { return NotFound("Project Not Found!"); }
        }
        //[HttpPost]
        //public async Task<IActionResult> CreateProject()
        //{            
        //    Project proj = new Project
        //    {  
        //        Id=0,
        //        Name = null,
        //        StartDate = DateTime.Now,
        //        EndDate=null,
        //        Total=null
        //    };
        //    await _context.Timely.AddAsync(proj);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetProject), new { id = proj.Id }, proj);
        //}

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateProject([FromRoute] int id, [FromBody] Project project)
        {
            var existingProject = from proj in _context.Timely
                                  where proj.Id == id
                                  select proj;

            foreach (Project detail in existingProject)
            {
                detail.Name = project.Name;
                if (detail.EndDate == null)
                {
                     detail.EndDate = DateTime.Now;
                     detail.Total = DateTime.Now - detail.StartDate;
                }
                    
                //else
                //    return Forbid("EndedAt Date can not be changed!");
            }
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
        //DeletingAProject
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteProject([FromRoute] int id)
        {
            var existingProject = from proj in _context.Timely
                                  where proj.Id == id
                                  select proj;

            
                if (existingProject.SingleOrDefault() != null)
            {
                _context.Timely.RemoveRange(_context.Timely.Where(x => x.Id == id));
                await _context.SaveChangesAsync();
                return Ok(existingProject);
            }
            return NotFound("Project not found!");
        }
        [HttpGet("StartSession")]
        public async Task<IActionResult> AddProject()
        {
            Project proj = new Project
            {
                Id = 0,
                Name = null,
                StartDate = DateTime.Now,
                EndDate = null,
                Total = null
            };
            await _context.Timely.AddAsync(proj);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = proj.Id }, proj);
        }
    }
}
