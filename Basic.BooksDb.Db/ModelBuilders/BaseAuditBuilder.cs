using Basic.BooksDb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basic.BooksDb.ModelBuilders
{
    class BaseAuditBuilder<T> : IEntityTypeConfiguration<T> where T : AuditBaseDb
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(p => p.CreatedBy).IsRequired();
            builder.Property(p => p.ModifiedBy).IsRequired();
            builder.Property(p => p.Created).HasDefaultValueSql("getutcdate()");
            builder.Property(p => p.ModifiedBy).HasDefaultValueSql("getutcdate()");
        }
    }
}
