public class TestStepService
{
    private readonly TestStepRepository _testStepRepository;

    public TestStepService(TestStepRepository testStepRepository)
    {
        _testStepRepository = testStepRepository;
    }

    public List<TestStep> GetAll() => _testStepRepository.GetAll();

    public List<TestStep> GetByTestCaseId(int testCaseId) => _testStepRepository.GetByTestCaseId(testCaseId);

    public TestStep GetById(int id) => _testStepRepository.GetById(id);

    public void Add(TestStep testStep) => _testStepRepository.Add(testStep);

    public void Update(int id, TestStep testStep) => _testStepRepository.Update(id, testStep);

    public void Delete(int id) => _testStepRepository.Delete(id);
}
