using Microsoft.EntityFrameworkCore;
using WebApplication1.Store;

namespace WebApplication1;

public class Query
{
    public IQueryable<Project> GetProjects([Service]AppDbContext dbContext)
    {
        return dbContext.Projects.Include( p => p.TimeLogs);
    }
    
    public async Task<Project?> GetProject(int id, [Service]AppDbContext dbContext)
    {
        var res = await dbContext.Projects.Include( p => p.TimeLogs).FirstOrDefaultAsync( c => c.Id == id);
        return res;
    }

    public IQueryable<TimeLog> GetTimeLogs([Service]AppDbContext dbContext)
    {
        return dbContext.TimeLogs;
    }
}