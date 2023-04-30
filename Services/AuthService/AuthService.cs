using server.Data;
namespace server.Services.AuthService

{
    public class AuthService : IAuthService
    {
        private readonly AuthContext _context;
        public AuthService(AuthContext context)
        {
            _context = context;
        }
        public async Task<string?> FindUser(UserDto dto)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (dbUser is null) return null;
            if (BCrypt.Net.BCrypt.Verify(dto.Password, dbUser.Password)) return dbUser.Id.ToString();
            return null;
        }


        public async Task<string?> Register(UserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Username = dto.Email?.Split("@")[0],
            };

            _context.Users.Add(newUser);
            try
            {
                await _context.SaveChangesAsync();
                return "User Created";
            }
            catch
            {
                return null;
            }

        }

        public async Task<User?> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if(user is null) return null;
            return user;
        }
    }
}