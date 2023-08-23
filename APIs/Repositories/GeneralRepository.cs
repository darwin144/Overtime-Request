using APIs.Context;
using APIs.Contract;

namespace APIs.Repositories
{
    public abstract class GeneralRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        protected readonly PayrollOvertimeContext _context;

        public GeneralRepository(PayrollOvertimeContext context)
        {
            _context = context;
        }

        public TEntity? Create(TEntity item)
        {
            try
            {
                _context.Set<TEntity>().Add(item);
                _context.SaveChanges();
                return item;
            }
            catch {
                return null;                     
            }
        }
        public bool Delete(Guid guid)
        {
            try
            {
                var entity = GetByGuid(guid);
                if (entity is null)
                {
                    return false;
                }
                _context.Set<TEntity>().Remove(entity);
                _context.SaveChanges();
                return true;
            }
            catch {
                return false;
            }
        }
        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }
        public TEntity? GetByGuid(Guid guid)
        {
            try
            {
                var entity = _context.Set<TEntity>().Find(guid);
                _context.ChangeTracker.Clear();
                return entity;
            }
            catch {
                return null;
            }
        }
        public bool Update(TEntity item)
        {
            try
            {
                var guid = (Guid)typeof(TEntity).GetProperty("Id").GetValue(item);
                var oldEntity = GetByGuid(guid);
                if (oldEntity is null)
                {
                    return false;
                }
                _context.Set<TEntity>().Update(item);
                _context.SaveChanges();
                return true;
            }
            catch {
                return false;
            }
        }
    }
}
