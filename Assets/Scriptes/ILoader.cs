
public interface ILoader
{
    public int Priority { get; set; }
    public void Load(string _path);
}
