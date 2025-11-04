namespace Api.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly Context _context;

        public Repository(Context context)
        {
            _context = context;
        }

        public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = filter == null ? _context.Set<T>() : _context.Set<T>().Where(filter);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Utiliza AsNoTracking() si no planeas modificar la entidad recuperada.
            return await query.AsNoTracking().FirstOrDefaultAsync(filter);
        }
        public async Task<T> AddAsync(T modelo)
        {
            try
            {
                await _context.Set<T>().AddAsync(modelo);
                await _context.SaveChangesAsync();
                return modelo;
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> UpdateAsync(T modelo)
        {
            try
            {
                _context.Set<T>().Update(modelo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch 
            {
                return false;
            }
            
        }
        public async Task<bool> UpdateRange(IQueryable<T> modelo)
        {
            try
            {
                _context.Set<T>().UpdateRange(modelo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
        public async Task<bool> Delete(T modelo)
        {
            try
            {
                _context.Set<T>().Remove(modelo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}
