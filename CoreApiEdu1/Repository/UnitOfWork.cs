using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApiEdu1.Entities;
using CoreApiEdu1.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace CoreApiEdu1.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BarcodeContext _context;
        private IGenericRepository<Cart> _carts;
        private IGenericRepository<Product> _products;
        private IGenericRepository<Production> _productions;
        private IGenericRepository<Machine> _machine;
        private IGenericRepository<MStop> _mStop;




        public UnitOfWork(BarcodeContext context)
        {
            _context = context;
        }

        public IGenericRepository<Cart> carts => _carts ??= new GenericRepository<Cart>(_context);
        public IGenericRepository<Product> products => _products ??= new GenericRepository<Product>(_context);
        public IGenericRepository<Production> productions => _productions ??= new GenericRepository<Production>(_context);
        public IGenericRepository<Machine> machines => _machine ??= new GenericRepository<Machine>(_context);
        public IGenericRepository<MStop> mStop => _mStop ??= new GenericRepository<MStop>(_context);




        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
