using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartE_Commerce_Data.Contracts
{
    public interface IRepository<T> where T : class
    {
        Task InsertAsync(T entity);


    }
}
