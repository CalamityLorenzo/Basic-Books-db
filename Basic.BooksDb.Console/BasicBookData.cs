using BooksDb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basic.BooksDb.Models
{
    public static class BasicBookData
    {
        public static IEnumerable<Book> Books() => new List<Book>
        {
            new Book("Book One", "Paul Lawrence", 1987, "This is the back of the book", DateTime.Parse("01/01/1987")),
            new Book("Book Two", "Jenny Bookface", 1983, "What we say about a new tome", DateTime.Parse("03/06/1983")
                        , new List<Review>
                        {
                            new Review("Written in spite", 8),
                            new Review("Pre vengent tengent", 0),
                            new Review("Caustic winds", 18)
                        }),
            new Book("A book in rhyme", "Floral Coral", 2007, "All up tin the beats", DateTime.Parse("14/05/2007")
            , new List<Review>
                        {
                            new Review("Nice Weather for ducks", 3),
                        })

        };
    }
}
