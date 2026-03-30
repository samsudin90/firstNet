public class TestCaseService
{
    private readonly TestCaseRepository _testCaseRepository;

    public TestCaseService(TestCaseRepository testCaseRepository)
    {
        _testCaseRepository = testCaseRepository;
    }

    public List<TestCase> GetAll() => _testCaseRepository.GetAll();

    public List<TestCase> GetByProjectId(int projectId) => _testCaseRepository.GetByProjectId(projectId);

    public TestCase GetById(int id) => _testCaseRepository.GetById(id);

    public void Add(TestCase testCase) => _testCaseRepository.Add(testCase);

    public void Update(int id, TestCase testCase) => _testCaseRepository.Update(id, testCase);

    public void Delete(int id) => _testCaseRepository.Delete(id);
}
