namespace Domain.Models;

public interface IMenuItem
{
    public string Name { get; set; }
    public string Url { get; set; }
    public IList<string> Roles { get; set; }
    //Submenu support
}
