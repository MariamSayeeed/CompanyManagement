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
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
        void Add(TEntity department);
        void Update(TEntity department);
        void Delete(TEntity   department);
        //IEnumerable<T> GetByName(string name);
    }
}
