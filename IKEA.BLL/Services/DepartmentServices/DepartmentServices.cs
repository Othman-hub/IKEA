using IKEA.DAL.Persistance.Repositories.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Services.DepartmentServices
{
    public class DepartmentServices:IDepartmentServices 
    {
        private IDepartmentRepository Repository;
        public DepartmentServices(IDepartmentRepository repository) => Repository = repository;

    }
}
