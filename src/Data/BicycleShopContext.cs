using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class BicycleShopContext : DbContext
    {
        public IBicycleRepository Bicycles => LoadRepo(() => new BicycleRepository(this));
        public IPartRepository Parts => LoadRepo(() => new PartRepository(this));
        
        public BicycleShopContext(DbContextOptions<BicycleShopContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bicycle>(entity =>
            {
                entity.Property(p => p.Id).UseSqlServerIdentityColumn();
                entity.Property(p => p.Title).IsRequired();
                entity.Property(p => p.Price).IsRequired();
                entity.Property(p => p.Quantity).HasDefaultValue(0).IsRequired();
                entity.Property(p => p.Discontinued).HasDefaultValue(false).IsRequired();

                entity.HasMany(d => d.BicycleParts)
                    .WithOne(p => p.Bicycle)
                    .HasForeignKey(d => d.BicycleId);
            });

            modelBuilder.Entity<BicyclePart>(entity =>
            {
                entity.HasKey(t => new {t.BicycleId, t.PartId}).ForSqlServerIsClustered();

                entity.HasOne(p => p.Bicycle)
                    .WithMany(d => d.BicycleParts);

                entity.HasOne(p => p.Part)
                    .WithMany(d => d.BicycleParts);
            });

            modelBuilder.Entity<Part>(entity =>
            {
                entity.Property(p => p.Id).UseSqlServerIdentityColumn();
                entity.Property(p => p.Title).IsRequired();
                entity.Property(p => p.Price).IsRequired();
                entity.Property(p => p.Quantity).HasDefaultValue(0).IsRequired();
                entity.Property(p => p.Discontinued).HasDefaultValue(false).IsRequired();
                
                entity.HasMany(d => d.BicycleParts)
                    .WithOne(p => p.Part)
                    .HasForeignKey(d => d.PartId);
            });
        }

        private readonly Dictionary<string, object> _repos = new Dictionary<string, object>();

        private TRepo LoadRepo<TRepo>(Expression<Func<TRepo>> expression)
        {
            var key = expression.Body.ToString();

            if (!_repos.TryGetValue(key, out var repo))
            {
                repo = _repos[key] = expression.Compile().Invoke();
            }

            return (TRepo) repo;
        }
    }
}