using WebApplication1.Store;

namespace WebApplication1;

public class Query
{
    public IQueryable<Project> GetProjects(AppDbContext dbContext) => dbContext.Projects;
    public IQueryable<TimeLog> GetTimeLogs(AppDbContext dbContext) => dbContext.TimeLogs;
}