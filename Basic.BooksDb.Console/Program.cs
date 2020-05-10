using Basic.BooksDb.Models;
using Basic.BooksDb.Repositories;
using BooksDb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Basic.BooksDb.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            Migrate();
            Update();
        }

        public static void Migrate()
        {
            var dbConn = File.ReadAllText("DbConnection.txt");

            var dbOpts = new DbContextOptionsBuilder().UseSqlServer(dbConn);
            using(var ctx = new BooksDbContext(dbOpts.Options))
            {
                ctx.Database.Migrate();
                SeedData sd = new SeedData(ctx, "PAul LAwrence");
                sd.Seed(BasicBookData.Books());
            }
        }

        public static void Update()
        {

            var bookid = Guid.Parse("ec7b5fae-2671-4eb8-9763-b69759530230");

            var dbConn = File.ReadAllText("DbConnection.txt");

            var dbOpts = new DbContextOptionsBuilder().UseSqlServer(dbConn);
            using (var ctx = new BooksDbContext(dbOpts.Options))
            {
                ctx.Database.Migrate();
                var repo = new BooksRepository(ctx, "Dericak smalls");

                var book = repo.GetBook(bookid);
                book.WithReviews(Enumerable.Empty<Review>());
                repo.UpdateBook(book);
            }
        }

        
    }
}
