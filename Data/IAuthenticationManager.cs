namespace webapi.Data
{
    public interface IAuthenticationManager
    {
        string Authenticate(string email, string password);
    }
}