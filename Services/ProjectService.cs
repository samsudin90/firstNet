public class ProjectService
{
    private readonly ProjectRepository _projectRepository;

    public ProjectService(ProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public List<Project> GetAll() => _projectRepository.GetAll();
    public Project GetById(int id) => _projectRepository.GetById(id);
    public void Add(Project project) => _projectRepository.Add(project);
    public void Update(int id, Project project) => _projectRepository.Update(id, project);
    public void Delete(int id) => _projectRepository.Delete(id);
}