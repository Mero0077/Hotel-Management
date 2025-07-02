using Hotel_Management.Data;
using Hotel_Management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Hotel_Management.Repositories
{
    public class GeneralRepository<T> where T : BaseModel
    {
        private DbSet<T> _dbSet;
        private readonly ApplicationDbContext _context;

        public GeneralRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public IQueryable<T> GetAll()
        {
            return _dbSet.Where(e => !e.IsDeleted);
        }
        public IQueryable<T> Get(Expression<Func<T, bool>> expression)
        {
            var res = GetAll().Where(expression);
            return res;
        }
        public async Task<T> GetOneWithTrackingAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AsTracking().Where(expression).FirstOrDefaultAsync();
        }
        public async Task<T> GetOneByIdAsync(int Id)
        {
            return await _dbSet.AsTracking().Where(e => e.Id == Id).FirstOrDefaultAsync();
        }


        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }
            public async Task UpdateIncludeAsync(T entity, params string[] modifiedProperties)
            {
                if (!_dbSet.Any(x => x.Id == entity.Id))
                    return;

                var local = _dbSet.Local.FirstOrDefault(x => x.Id == entity.Id);

                EntityEntry entityEntry;
                if (local is null)
                {
                    entityEntry = _dbSet.Entry(entity);
                }
                else
                {
                    entityEntry = _context.ChangeTracker.Entries<T>().FirstOrDefault(x => x.Entity.Id == entity.Id);
                }

            foreach (var property in entityEntry.Properties)
            {
                if (modifiedProperties.Contains(property.Metadata.Name))
                {
                    property.CurrentValue = entity.GetType().GetProperty(property.Metadata.Name).GetValue(entity);
                    property.IsModified = true;
                }
            }
      }

        //public void UpdateReflection(T entity, params string[] modifiedproperties) //modidified dh el7agat eli bs h3mlha update      
        //{
        //    if (!_dbSet.Any(x => x.Id == entity.Id)) return; // if entity does not exist in dbset

        //    var local = _dbSet.Local.FirstOrDefault(e => e.Id == entity.Id);
        //    EntityEntry entityEntry;

        //    if (local == null)  // entity m4 m3molha tracking
        //    {
        //        entityEntry = _context.Entry(entity); // e3mlo tracking
        //    }
        //    else // lw m3molha tracking hatly metadata bta3tha
        //    {
        //        entityEntry = _context.ChangeTracker.Entries<T>().FirstOrDefault(x => x.Entity.Id == entity.Id);
        //    }

        //    foreach (var property in entityEntry.Properties)
        //    {
        //        if (modifiedproperties.Contains(property.Metadata.Name)) // if values in modifiedarray exists in the metdata??
        //        {
        //            property.CurrentValue = entity.GetType().GetProperty(property.Metadata.Name).GetValue(entity);
        //            property.IsModified = true;

        //        }
        //    }
        //    _context.SaveChanges();
        //}

        public async Task<T> DeleteAsync(int Id)
        {
            var res = await GetOneByIdAsync(Id);

            if (res != null || !res.IsDeleted)
            {
                res.IsDeleted = true;
            }
            return res;
        }

        public bool IsExists(int Id)
        {
            return GetOneByIdAsync(Id) != null ? true : false;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var isMatch = await _dbSet.AnyAsync(predicate, cancellationToken);

            return isMatch;
        }
    }
}
