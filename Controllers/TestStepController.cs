using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TestStepController : ControllerBase
{
    private readonly TestStepService _testStepService;

    public TestStepController(TestStepService testStepService)
    {
        _testStepService = testStepService;
    }

    [HttpGet]
    public ActionResult<List<TestStep>> GetAll()
    {
        return Ok(_testStepService.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult<TestStep> GetById(int id)
    {
        var testStep = _testStepService.GetById(id);
        if (testStep == null)
        {
            return NotFound();
        }
        return Ok(testStep);
    }

    [HttpGet("testcase/{testCaseId}")]
    public ActionResult<List<TestStep>> GetByTestCaseId(int testCaseId)
    {
        return Ok(_testStepService.GetByTestCaseId(testCaseId));
    }

    [HttpPost]
    public ActionResult Add(CreateTestStepRequest request)
    {
        var testStep = new TestStep
        {
            TestCaseId = request.TestCaseId,
            StepOrder = request.StepOrder,
            Action = request.Action,
            Selector = request.Selector,
            Value = request.Value
        };
        _testStepService.Add(testStep);
        return CreatedAtAction(nameof(GetById), new { id = testStep.Id }, testStep);
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, TestStep testStep)
    {
        _testStepService.Update(id, testStep);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _testStepService.Delete(id);
        return NoContent();
    }
}
