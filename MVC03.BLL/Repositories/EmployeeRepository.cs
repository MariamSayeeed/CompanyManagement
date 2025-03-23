using Microsoft.EntityFrameworkCore;
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
        private readonly CompanyDbContext _context;

        public EmployeeRepository(CompanyDbContext context) : base( context)  // Ask CLR to create Obj from CompanyDbcontext
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetByNameAsync(string name)
        {
            return await _context.Employees.Include(E=> E.Department)
                                     .Where(e => e.Name.ToLower()
                                     .Contains(name.ToLower())).ToListAsync();
        }
    }
}
