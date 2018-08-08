using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.DAL.Repositories
{
    public abstract class BaseRepository : IDisposable
    {

        #region Fields

        private readonly DbContext _context;
        private bool _disposeContext;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        protected BaseRepository(DbContext context, bool disposeContext = true)
        {
            _disposeContext = disposeContext;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region Protected members

        /// <summary>
        /// Executes the given SQL command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        protected void ExecuteSqlCommand(string query, params object[] parameters)
        {
            _context.Database.ExecuteSqlCommand(query, parameters);
        }

        /// <summary>
        /// Gets all the entities for the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected DbSet<T> All<T>() where T : class
        {
            return _context.Set<T>();
        }

        /// <summary>
        /// Alls the including.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">includeProperties</exception>
        protected IQueryable<T> AllIncluding<T>(params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            if (includeProperties == null)
                throw new ArgumentNullException("includeProperties");

            IQueryable<T> query = All<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Finds the entity by the specified id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        protected T Find<T>(int id) where T : class
        {
            return All<T>().Find(id);
        }

        protected T Find<T>(Guid id) where T : class
        {
            return All<T>().Find(id);
        }

        /// <summary>
        /// Finds the specified expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected T Find<T>(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            return AllIncluding(includeProperties).SingleOrDefault(expression);
        }

        /// <summary>
        /// Finds the specified expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected T FindOrCreate<T>(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includeProperties) where T : class, new()
        {
            return Find(expression, includeProperties) ?? new T();
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        protected virtual int SaveChanges()
        {
            return _context.SaveChanges();
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        protected async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets a list of the given entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// Returns a list of type <typeparamref name="T" />
        /// </returns>
        protected virtual IQueryable<T> List<T>() where T : class
        {
            return All<T>().AsQueryable();
        }

        /// <summary>
        /// Lists the specified expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        protected virtual IQueryable<T> List<T>(params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            return AllIncluding<T>(includeProperties).AsQueryable();
        }

        /// <summary>
        /// Adds the specified entity to the data context and saves it .
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected virtual int Add<T>(T entity) where T : class
        {
            All<T>().Add(entity);
            return SaveChanges();
        }

        /// <summary>
        /// Deletes the entity with the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        protected virtual void Delete<T>(int id) where T : class
        {
            var entity = Find<T>(id);
            Delete<T>(entity);
        }

        /// <summary>
        /// Deletes the specified entity from the datacontext.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected virtual int Delete<T>(T entity) where T : class
        {
            All<T>().Remove(entity);
            return SaveChanges();
        }

        /// <summary>
        /// Deletes the specified entity from the datacontext asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected async Task<int> DeleteAsync<T>(T entity) where T : class
        {
            All<T>().Remove(entity);
            return await SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        protected virtual int Delete<T>(IEnumerable<T> entities) where T : class
        {
            IEnumerable<T> list = new List<T>(entities);
            foreach (var entity in list)
            {
                Delete(entity);
            }
            return SaveChanges();
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null && _disposeContext)
                    _context.Dispose();
            }
        }

        #endregion
    }
}
