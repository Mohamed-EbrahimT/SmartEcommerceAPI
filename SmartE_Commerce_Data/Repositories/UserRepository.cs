using FinalProj.Data;
using FinalProj.Models;
using Microsoft.EntityFrameworkCore;
using SmartE_Commerce_Data.Contracts;

namespace SmartE_Commerce_Data.Repositories
{
    internal class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ECContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await db
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await db.AnyAsync(u => u.Email == email);
        }
    }
}
