namespace TokenBasedAuthentication.Configuration;

public interface DTOBase
{
    public List<string> Errors { get; set; } 
    public bool Succeeded { get; set; }
}