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
        IGenericRepository<Production> productions { get; }
        IGenericRepository<Machine> machines { get; }
        IGenericRepository<MStop> mStop { get; }



        Task Save();
    }
}
