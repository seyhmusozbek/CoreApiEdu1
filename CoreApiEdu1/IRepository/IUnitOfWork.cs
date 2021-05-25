using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApiEdu1.Entities;

namespace CoreApiEdu1.IRepository
{
    public interface IUnitOfWork: IDisposable
    {
        IGenericRepository<Cart> carts { get;}
        IGenericRepository<Product> products { get;}
        Task Save();
    }
}
