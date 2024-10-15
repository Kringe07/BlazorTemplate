using Microsoft.EntityFrameworkCore;
using ProjectName.DataAccess.DatabaseContext;
using ProjectName.Entities;

namespace ProjectName.DataAccess.Repository
{
    public class UserRepository
    {
        private readonly IDbContextFactory<ProjectNameContext> _context;

        public UserRepository(IDbContextFactory<ProjectNameContext> context)
        {
            _context = context;
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
