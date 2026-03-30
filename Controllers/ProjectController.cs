using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly ProjectService _projectService;

    public ProjectController(ProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public ActionResult<List<Project>> GetAll()
    {
        return Ok(_projectService.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult<Project> GetById(int id)
    {
        var project = _projectService.GetById(id);
        if (project == null)
        {
            return NotFound();
        }
        return Ok(project);
    }

    [HttpPost]
    public ActionResult Add(CreateProjectRequest request)
    {
        var project = new Project
        {
            Name = request.Name
        };
        _projectService.Add(project);
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, Project project)
    {
        _projectService.Update(id, project);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _projectService.Delete(id);
        return NoContent();
    }
}