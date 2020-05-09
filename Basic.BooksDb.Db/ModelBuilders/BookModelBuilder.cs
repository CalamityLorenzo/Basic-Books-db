using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using BooksDb.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooksDb.ModelBuilders
{
    class BookModelBuilder : IEntityTypeConfiguration<BookDb>
    {
        public void Configure(EntityTypeBuilder<BookDb> builder)
        {
            builder.Property(p => p.Id).HasDefaultValueSql("NEWID()");
            builder.Property(p => p.DatePublished).HasColumnType("date").IsRequired(true);
            builder.Property(p => p.Author).IsRequired();
            builder.Property(p => p.Name).IsRequired();
        }
    }
}
