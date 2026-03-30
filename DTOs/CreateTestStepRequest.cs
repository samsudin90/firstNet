public class CreateTestStepRequest
{
    public int TestCaseId { get; set; }
    public int StepOrder { get; set; }
    public string Action { get; set; }
    public string Selector { get; set; }
    public string Value { get; set; }
}
