namespace WebApplication1.Store;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CreatedBy { get; set; }
    
    public ICollection<TimeLog> TimeLogs { get; set; }
}