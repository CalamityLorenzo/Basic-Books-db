using Basic.BooksDb.ModelBuilders;
using BooksDb.Entities;
using BooksDb.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Basic.BooksDb.Repositories
{
    public class SeedData
    {
        private readonly BooksDbContext ctx;
        private readonly EntityAuditManager<BookDb, Guid> bookAuditManager;
        private readonly EntityAuditManager<ReviewDb, Guid> reviewsAuditManager;
        public SeedData(BooksDbContext ctx, string userName)
        {
            this.ctx = ctx;
            bookAuditManager = new EntityAuditManager<BookDb, Guid>(userName, p => p.Id.Equals(Guid.Empty), p => ctx.Books.Find(p.Id));
            reviewsAuditManager = new EntityAuditManager<ReviewDb, Guid>(userName, p => p.Id.Equals(Guid.Empty), p => ctx.Reviews.Find(p.Id));

        }

        public void Seed(IEnumerable<Book> boooooks)
        {

            if (ctx.Books.Count() > 0) return;

            var dbBooks = new List<BookDb>();
            foreach (var book in boooooks)
            {
                var dbBook = book.ToDb();
                bookAuditManager.SetAuditInfo(dbBook);
                foreach (var review in dbBook.Reviews)
                {
                    reviewsAuditManager.SetAuditInfo(review);
                }

                dbBooks.Add(dbBook);
            }

            ctx.AddRange(dbBooks);
            ctx.SaveChanges();
        }
    }
}
