using Basic.BooksDb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;

namespace Basic.BooksDb.Repositories
{/// <summary>
///  This dies not manage collections.
/// </summary>
/// <typeparam name="BaseDbEntity"></typeparam>
    internal abstract class BaseAuditRepository<BaseDbEntity> where BaseDbEntity : AuditBaseDb, new()
    {
        protected readonly DbContext ctx;
        private readonly string userName;

        public BaseAuditRepository(DbContext ctx, string userName)
        {
            this.ctx = ctx;
            this.userName = userName;
        }

    /// <summary>
    /// Test to see if the passed entity is a new record. (This usually amounts to testing the id)
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
        protected abstract bool IsNewRecord(BaseDbEntity entity);
        // Get the original entity of an update. 
        // so we can recover the original audit info.
        protected abstract AuditBaseDb FetchOriginalAudit(BaseDbEntity entity);

        private void UpdateAuditInfo(BaseDbEntity entity, DateTime utcPlease)
        {
            entity.Modified = utcPlease;
            entity.ModifiedBy = userName;
        }
        private void NewEntity(BaseDbEntity entity, DateTime utcPlease)
        {
            entity.Created = utcPlease;
            entity.CreatedBy = userName;
        }
        
        // Whhat we use publically.
        protected void NewEntity(BaseDbEntity entity)
        {
            NewEntity(entity, DateTime.UtcNow);
            UpdateAuditInfo(entity, DateTime.UtcNow);
        }
        protected void UpdateAuditInfo(BaseDbEntity entity)
        {
            this.UpdateAuditInfo(entity, DateTime.UtcNow);
        }

        protected void MigrateAudit(BaseDbEntity updated, AuditBaseDb original)
        {
            updated.Created = original.Created;
            updated.CreatedBy = original.CreatedBy;
        }

        protected void ConfigureAuditData(IEnumerable<BaseDbEntity> updated, IEnumerable<BaseDbEntity> original )
        {
            var originalItems = original.ToList();
            foreach(var item in updated)
            {
                
            }
        }

        protected BaseDbEntity SaveEntity(BaseDbEntity entity)
        {
            if (!IsNewRecord(entity))
            {
                var original = FetchOriginalAudit(entity);
                MigrateAudit(entity, original);

            }
            var savedEntity = this.ctx.Update<BaseDbEntity>(entity);
            this.ctx.SaveChanges();
            return savedEntity.Entity;
        }
    }
}
