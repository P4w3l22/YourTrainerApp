namespace YourTrainer_App.Models;

public class Blog
{
    public string Category { get; set; }
    public string Title { get; set; }
    public List<string> Users { get; set; }
    public List<string> Comments { get; set; }
}
