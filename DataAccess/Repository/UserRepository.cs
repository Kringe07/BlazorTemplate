using Microsoft.EntityFrameworkCore;
using ProjectName.DataAccess.DatabaseContext;
using ProjectName.DataAccess.Entities;

namespace ProjectName.DataAccess.Repository
{
    public class UserRepository
    {
        private readonly IDbContextFactory<ProjectNameContext> _context;

        public UserRepository(IDbContextFactory<ProjectNameContext> context)
        {
            _context = context;
        }
        public async Task<List<T>> GetUsers<T>() where T : User
        {
            await using var context = await _context.CreateDbContextAsync();
            return await context.Users.OfType<T>().ToListAsync();
        }

        public async Task<T> GetUser<T>(Guid id) where T : User
        {
            await using var context = await _context.CreateDbContextAsync();
            var user = await context.Users.OfType<T>().FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new Exception("User not found!");

            return user;
        }
        public async Task AddUser<T>(T user) where T : User
        {
            await using var context = await _context.CreateDbContextAsync();
            context.Add(user);
            await context.SaveChangesAsync();
        }
        public async Task<User?> GetUserDataAsync(string Email)
        {
            try
            {
                await using ProjectNameContext context = await _context.CreateDbContextAsync();
                var x = await context.Users.FirstOrDefaultAsync(x => x.Email == Email);
                return x;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
