using BooksDb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basic.BooksDb.Console
{
    public static class BookWithExtension
    {
        public static Book WithAuthor(this Book @this, string newAuthor)
        {
            return new Book(id: @this.Id, name: @this.Name, year: @this.Year, author: newAuthor,
                            blurb: @this.Blurb, datePublished: @this.DatePublished, reviews: @this.Reviews
                            );
        }

        public static Book WithDatePublished(this Book @this, DateTime newDatePublished)
        {
            return new Book(id: @this.Id, name: @this.Name, year: @this.Year, author: @this.Author,
                            blurb: @this.Blurb, datePublished: newDatePublished, reviews: @this.Reviews
                            );
        }

        public static Book WithReviews(this Book @this, IEnumerable<Review> newReviews)
        {
            return new Book(id: @this.Id, name: @this.Name, year: @this.Year, author: @this.Author,
                            blurb: @this.Blurb, datePublished: @this.DatePublished, reviews: newReviews
                            );
        }
    }


}
