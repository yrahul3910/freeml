using MLServer.Domain.Interfaces;
using MLServer.Infra.Data.Context;

namespace MLServer.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MLServerContext _context;

        public UnitOfWork(MLServerContext context)
        {
            _context = context;
        }

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}