using MVC03.BLL.Interfaces;
using MVC03.DAL.Data.Contexts;
using MVC03.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC03.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee> , IEmployeeRepository
    {

        public EmployeeRepository(CompanyDbContext context) : base( context)  // Ask CLR to create Obj from CompanyDbcontext
        {
            
        }
    }
}
