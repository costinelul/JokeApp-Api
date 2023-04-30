namespace server.Services.AuthService
{
    public interface IAuthService
    {
        Task<string?> Register(UserDto userInput);
        Task<string?> FindUser(UserDto userInput);
        Task<User?> GetUser(int id);
    }
}