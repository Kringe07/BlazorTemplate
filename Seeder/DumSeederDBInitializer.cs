using Bogus;
using Microsoft.EntityFrameworkCore;
using OtpNet;
using ProjectName.DataAccess.DatabaseContext;
using ProjectName.DataAccess.Entities;
using BC = BCrypt.Net.BCrypt;

namespace ProjectName.Seeder
{
    public static class DumSeederDBInitializer
    {
        public static async void Seed(IServiceProvider serviceProvider)
        {
            using var context = new ProjectNameContext(serviceProvider.GetRequiredService<DbContextOptions<ProjectNameContext>>());
            if (!context.Users.OfType<Customer>().Any())
            {
                List<Customer> customers = await CustomerSeeder(10);
                context.Users.AddRange(customers);
            }
            if (!context.Users.OfType<Admin>().Any())
            {
                List<Admin> admins = await AdminSeeder(5);
                context.Users.AddRange(admins);
            }
            await context.SaveChangesAsync();
        }
        public static async Task<List<Customer>> CustomerSeeder(int Amount)
        {
            Faker<Customer> GenerateCustomer = new Faker<Customer>()
                             .StrictMode(true)
                             .RuleFor(u => u.Id, f => Guid.NewGuid())
                             .RuleFor(u => u.Email, f => f.Person.Email)
                             .RuleFor(u => u.Password, f => BC.EnhancedHashPassword("password"))
                             .RuleFor(u => u.SecretKey, f => GenerateSecretKey());
            List<Customer> s = GenerateCustomer.Generate(Amount);
            return await Task.FromResult(s);
        }

        public static async Task<List<Admin>> AdminSeeder(int Amount)
        {
            Faker<Admin> GenerateAdmin = new Faker<Admin>()
                 .StrictMode(true)
                 .RuleFor(u => u.Id, f => Guid.NewGuid())
                 .RuleFor(u => u.Email, f => f.Person.Email)
                 .RuleFor(u => u.Password, f => BC.EnhancedHashPassword("password"))
                 .RuleFor(u => u.SecretKey, f => GenerateSecretKey());
            List<Admin> s = GenerateAdmin.Generate(Amount);
            return await Task.FromResult(s);
        }

        // Generate secret key for user
        public static string GenerateSecretKey()
        {
            byte[] key = KeyGeneration.GenerateRandomKey(20);
            return Base32Encoding.ToString(key);
        }
    }
}
