using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Basic.BooksDb.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            Migrate();
        }

        public static void Migrate()
        {
            var dbConn = File.ReadAllText("DbConnection.txt");

            var dbOpts = new DbContextOptionsBuilder().UseSqlServer(dbConn);
            using(var ctx = new BooksDbContext(dbOpts.Options))
            {
                ctx.Database.Migrate();
            }
        }

        
    }
}
