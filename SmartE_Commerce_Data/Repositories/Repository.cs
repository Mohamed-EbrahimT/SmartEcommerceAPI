using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProj.Data;
using Microsoft.EntityFrameworkCore;
using SmartE_Commerce_Data.Contracts;

namespace SmartE_Commerce_Data.Repositories
{
    internal class Repository<T> : IRepository<T> where T : class
        //internal abstract class Repository<T> : IRepository<T> where T : class المشكلة طلعت في الابستراكت بيعمل مشاكل مع الدبيندنسي انجكشن

    {
        protected readonly ECContext context;
        protected readonly DbSet<T> db;

        public Repository(ECContext _context)
        {
            context = _context;
            db = context.Set<T>();
        }

        public async Task InsertAsync(T entity)
        {
            await db.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await db.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            db.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                db.Remove(entity);
                await context.SaveChangesAsync();
            }
        }


    }
}
