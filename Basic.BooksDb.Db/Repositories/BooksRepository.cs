using BooksDb.Entities;
using BooksDb.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Basic.BooksDb.Repositories
{
    public class BooksRepository
    {
        private readonly BooksDbContext _ctx;
        private readonly EntityAuditManager<ReviewDb, Guid> reviewsAuditManager;
        private readonly EntityAuditManager<BookDb, Guid> booksAuditManager;

        public BooksRepository(BooksDbContext ctx, string userName)
        {
            this._ctx = ctx;

            this.reviewsAuditManager = new EntityAuditManager<ReviewDb, Guid>(userName, r => r.Id.Equals(Guid.Empty), p => ctx.Reviews.AsNoTracking().FirstOrDefault(a=>a.Id==p.Id) );
            this.booksAuditManager = new EntityAuditManager<BookDb, Guid>(userName, r => r.Id.Equals(Guid.Empty), p => ctx.Books.AsNoTracking().FirstOrDefault(a=>a.Id==p.Id));
        }

        /// <summary>
        /// You can also add reviews at the same time.
        /// </summary>
        /// <param name="newBook"></param>
        public Book AddBook(Book newBook)
        {
            var newBookDb = newBook.ToDb();
            booksAuditManager.SetAuditInfo(newBookDb);

            reviewsAuditManager.SetAuditInfo(newBookDb.Reviews.ToList());
            var saved = this._ctx.Add(newBookDb);
            _ctx.SaveChanges();
            return saved.Entity.ToClient();
        }

        public Book GetBook(Guid id)
        {
            return _ctx.Books.AsNoTracking().First(p => p.Id == id).ToClient();
        }

        public Review AddBookReview(Guid Id, Review review)
        {
            var newReview = review.ToDb();
            newReview.BookId = Id;
            reviewsAuditManager.SetAuditInfo(newReview);
            var reviewTracking = this._ctx.Reviews.Add(newReview);
            _ctx.SaveChanges();

            return reviewTracking.Entity.ToClient();

        }

        /// <summary>
        /// Updating a book, means updating the reviews too.
        /// So if the review is MISSING in the client version
        /// It is DELETED in the Server version.
        /// </summary>
        /// <param name="book"></param>
        public void UpdateBook(Book book)
        {
            var dbBook = book.ToDb();
            // update all the reviews to have at least the correct bookId
            // not doing this means NEW reviews are skipped.
            dbBook.Reviews = dbBook.Reviews.Select(o => { o.BookId = book.Id; return o; }).ToList();
            // Fetch all the original reviews and find out if any are missing.
            var allReviews = _ctx.Reviews.Where(r => r.BookId == dbBook.Id).ToList();
            var removedReviews = allReviews.Where(a => !dbBook.Reviews.Any(db=> db.Id != a.Id)).ToList();

            booksAuditManager.SetAuditInfo(dbBook);
            reviewsAuditManager.SetAuditInfo(dbBook.Reviews.ToList(), _ctx.Reviews.Where(r=>r.BookId==dbBook.Id));
            _ctx.Reviews.RemoveRange(removedReviews);
            _ctx.Books.Update(dbBook);
            _ctx.SaveChanges();
        }

        public void DropBook(Guid bookId)
        {
            var book = _ctx.Books.First(p => p.Id == bookId);
            _ctx.Books.Remove(book);
            _ctx.SaveChanges();
        }

        public void DropBook(Book book)
        {
            this.DropBook(book.Id);
        }

        public void DropReview(Guid ReviewId)
        {
            var review = _ctx.Reviews.First(p => p.Id == ReviewId);
            _ctx.Reviews.Remove(review);
            _ctx.SaveChanges();
        }

        public void DropReview(Review review)
        {
            this.DropReview(review.Id);
        }

    }
}
