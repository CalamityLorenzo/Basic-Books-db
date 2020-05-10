using Basic.BooksDb.Entities;
using Basic.BooksDb.Migrations;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basic.BooksDb.Repositories
{
    internal class EntityAuditManager<BaseDbEntity, T> where BaseDbEntity : AuditBaseDb, IEntityDbId<T>, new()
    {
        private readonly Func<BaseDbEntity, bool> isNewRecord;
        private readonly Func<BaseDbEntity, AuditBaseDb> originalItemAudit;
        private readonly string userName;

        public EntityAuditManager(string userName, Func<BaseDbEntity, bool> isNewRecord, Func<BaseDbEntity, AuditBaseDb> originalItemAudit)
        {
            this.isNewRecord = isNewRecord;
            this.originalItemAudit = originalItemAudit;
            this.userName = userName;
        }
   
        public void SetAuditInfo(BaseDbEntity newRecord)
        {
            if (!isNewRecord(newRecord))
            {
                var original = originalItemAudit(newRecord);
                var adate = DateTime.UtcNow;
                MigrateAudit(newRecord, original);
                UpdateAuditInfo(newRecord, adate);
            }
            else
            {
                var cDate = DateTime.UtcNow;
                NewEntity(newRecord, cDate);
                UpdateAuditInfo(newRecord, cDate);
            }

        }

        public void SetAuditInfo(BaseDbEntity updated, BaseDbEntity original)
        {
            var cDate = DateTime.UtcNow;
            MigrateAudit(updated, original);
            UpdateAuditInfo(updated, cDate);
        }

        public void SetAuditInfo(List<BaseDbEntity> updated, IEnumerable<BaseDbEntity> original)
        {
            var allOriginal = original.ToList();
            foreach(var update in updated)
            {
                if(!isNewRecord(update))
                {
                    var origin = allOriginal.First(f => f.Id.Equals(update.Id));
                    SetAuditInfo(updated, original);
                }
                else
                {
                    NewAuditInfo(update);
                    UpdateAuditInfo(update);
                }

            }
        }

        public void SetAuditInfo(List<BaseDbEntity> newItems)
        {
            SetAuditInfo(newItems, Enumerable.Empty<BaseDbEntity>());
        }


        protected void MigrateAudit(BaseDbEntity updated, AuditBaseDb original)
        {
            updated.Created = original.Created;
            updated.CreatedBy = original.CreatedBy;
        }

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
        protected void NewAuditInfo(BaseDbEntity entity)
        {
            NewEntity(entity, DateTime.UtcNow);
            UpdateAuditInfo(entity, DateTime.UtcNow);
        }
        protected void UpdateAuditInfo(BaseDbEntity entity)
        {
            this.UpdateAuditInfo(entity, DateTime.UtcNow);
        }

    }
}
