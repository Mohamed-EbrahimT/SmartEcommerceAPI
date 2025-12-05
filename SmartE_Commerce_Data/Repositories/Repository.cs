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
        private readonly ECContext context;
        private readonly DbSet<T> db;

        public Repository(ECContext _context)
        {
            context = _context;
            db=context.Set<T>();
        }


        public async Task InsertAsync(T entity)
        {
            await db.AddAsync(entity);
            await context.SaveChangesAsync();
        }

    }
}
