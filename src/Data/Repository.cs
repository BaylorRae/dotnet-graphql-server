using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity FindById(object id);
        
        int Insert(TEntity entity);
        void Insert(IEnumerable<TEntity> entities);
        
        int Update(TEntity entity);
        int Delete(TEntity entity);

        IEnumerable<TEntity> All();
    }

    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private BicycleShopContext Db { get; }
        protected DbSet<TEntity> Set => Db.Set<TEntity>();

        protected Repository(BicycleShopContext db)
        {
            Db = db;
        }

        public TEntity FindById(object id)
        {
            return Set.Find(id);
        }

        public int Insert(TEntity entity)
        {
            Set.Add(entity);
            return Db.SaveChanges();
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            Set.AddRange(entities);
            Db.SaveChanges();
        }

        public int Update(TEntity entity)
        {
            Set.Update(entity);
            return Db.SaveChanges();
        }

        public int Delete(TEntity entity)
        {
            Set.Remove(entity);
            return Db.SaveChanges();
        }

        public IEnumerable<TEntity> All()
        {
            return Set.ToList();
        }
    }
}