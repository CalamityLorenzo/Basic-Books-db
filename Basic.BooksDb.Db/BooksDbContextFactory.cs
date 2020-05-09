using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Basic.BooksDb
{
    public class BooksDbContextFactory : IDesignTimeDbContextFactory<BooksDbContext>
    {
        public BooksDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BooksDbContext>();
            var dbCon = File.ReadAllText("DbConnection.txt");
            optionsBuilder.UseSqlServer(dbCon);
            return new BooksDbContext(optionsBuilder.Options);
        }
    }
}
