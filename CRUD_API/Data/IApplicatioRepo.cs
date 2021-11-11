using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_API.Data
{
    public interface IApplicatioRepo<T> where T : class 
    {
        Task<bool> SaveChange();
        void Update(T employeeModel);
        void Delete(T employee);
    }
}
