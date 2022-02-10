namespace TokenBasedAuthentication.Configuration;

public class AuthResult
{
    public string Token { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new List<string>();
    public bool Succeeded { get; set; } = false;
}