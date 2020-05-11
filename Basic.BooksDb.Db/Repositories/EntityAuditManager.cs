using Basic.BooksDb.Entities;
using Basic.BooksDb.Migrations;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basic.BooksDb.Repositories
{
    /// <summary>
    /// This just manages current, and existing audit settings.
    /// The use of isNewRecord, and OriginalItem, are convienice.
    /// Moving them out of the repoistory makes it less 'cluttereed' everytime you want to save.
    /// The key here is convience for the implementer.
    /// </summary>
    /// <typeparam name="BaseDbEntity"></typeparam>
    /// <typeparam name="T"></typeparam>
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
            // Existing record, fetch the original
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
            foreach (var update in updated)
            {
                if (!isNewRecord(update))
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
            // If there are non-new items it WILL crash.
            SetAuditInfo(newItems, Enumerable.Empty<BaseDbEntity>());
        }

        /// <summary>
        /// Copy the db Created audit data into a new record.
        /// </summary>
        /// <param name="updated"></param>
        /// <param name="original"></param>
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
