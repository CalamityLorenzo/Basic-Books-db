using Microsoft.EntityFrameworkCore;
using System;
using BooksDb.Entities;
using BooksDb.ModelBuilders;

namespace Basic.BooksDb
{
    public class BooksDbContext : DbContext
    {
        public BooksDbContext(DbContextOptions options) : base(options)
        {

        }

        protected BooksDbContext()
        {
        }

        internal  DbSet<BookDb> Books { get; set; }
        internal DbSet<ReviewDb> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookModelBuilder());
            modelBuilder.ApplyConfiguration(new ReviewModelBuilder());
        }
    }
}
