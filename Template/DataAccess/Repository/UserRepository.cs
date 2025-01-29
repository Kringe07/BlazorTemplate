using Microsoft.EntityFrameworkCore;
using ProjectName.DataAccess.DatabaseContext;
using ProjectName.DataAccess.Entities;

namespace ProjectName.DataAccess.Repository;

public class UserRepository(IDbContextFactory<ProjectNameContext> context)
{
    // Get all Users
    public async Task<List<T>> GetUsers<T>() where T : User
    {
        await using var context1 = await context.CreateDbContextAsync();
        return await context1.Users.OfType<T>().ToListAsync();
    }

    // Get 1 User with Id
    public async Task<T> GetUser<T>(Guid id) where T : User
    {
        await using var context1 = await context.CreateDbContextAsync();
        var user = await context1.Users.OfType<T>().FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            throw new Exception("User not found!");

        return user;
    }

    // Add new User
    public async Task AddUser<T>(T user) where T : User
    {
        await using var context1 = await context.CreateDbContextAsync();
        context1.Add(user);
        await context1.SaveChangesAsync();
    }

    //Get 1 User with Email
    public async Task<User?> GetUserDataAsync(string email)
    {
        try
        {
            await using var context1 = await context.CreateDbContextAsync();
            return await context1.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}