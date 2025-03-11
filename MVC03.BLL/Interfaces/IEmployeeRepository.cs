using MVC03.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC03.BLL.Interfaces
{
    public interface IEmployeeRepository 
    {
        IEnumerable<Employee> GetAll();
        Employee Get(int id);
        int Add(Employee department);
        int Update(Employee department);
        int Delete(Employee department);
    }
}
