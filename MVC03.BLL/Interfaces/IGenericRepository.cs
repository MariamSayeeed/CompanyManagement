using MVC03.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC03.BLL.Interfaces
{
    public interface IGenericRepository <TEntity> where TEntity :  BaseEntity 
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetAsync(int id);
        Task AddAsync(TEntity department);
        void Update(TEntity department);
        void Delete(TEntity   department);
        //IEnumerable<T> GetByName(string name);
    }
}
