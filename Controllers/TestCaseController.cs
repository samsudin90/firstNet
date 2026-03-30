using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TestCaseController : ControllerBase
{
    private readonly TestCaseService _testCaseService;

    public TestCaseController(TestCaseService testCaseService)
    {
        _testCaseService = testCaseService;
    }

    [HttpGet]
    public ActionResult<List<TestCase>> GetAll()
    {
        return Ok(_testCaseService.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult<TestCase> GetById(int id)
    {
        var testCase = _testCaseService.GetById(id);
        if (testCase == null)
        {
            return NotFound();
        }
        return Ok(testCase);
    }

    [HttpGet("project/{projectId}")]
    public ActionResult<List<TestCase>> GetByProjectId(int projectId)
    {
        return Ok(_testCaseService.GetByProjectId(projectId));
    }

    [HttpPost]
    public ActionResult Add(CreateTestCaseRequest request)
    {
        var testCase = new TestCase
        {
            ProjectId = request.ProjectId,
            Name = request.Name
        };
        _testCaseService.Add(testCase);
        return CreatedAtAction(nameof(GetById), new { id = testCase.Id }, testCase);
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, TestCase testCase)
    {
        _testCaseService.Update(id, testCase);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _testCaseService.Delete(id);
        return NoContent();
    }
}
