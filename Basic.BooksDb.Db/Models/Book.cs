using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooksDb.Models
{
    public class Book
    {
        public Book(Guid id, string name, string author, int year, string blurb, DateTime datePublished, IEnumerable<Review> reviews)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Author = author ?? throw new ArgumentNullException(nameof(author));
            Year = year;
            Blurb = blurb ?? throw new ArgumentNullException(nameof(blurb));
            DatePublished = datePublished;
            Reviews = new List<Review>(reviews);
        }
// no id
        public Book(string name, string author, int year, string blurb, DateTime datePublished, IEnumerable<Review> reviews) : this(Guid.Empty, name, author, year, blurb, datePublished, reviews)
        { }
        // no id no reviews
        public Book(string name, string author, int year, string blurb, DateTime datePublished) : this(Guid.Empty, name, author, year, blurb, datePublished, Enumerable.Empty<Review>())
        { }

        public Guid Id {get;}
        public string Name {get;}
        public string Author {get;}
        public int Year {get;}
        public string Blurb {get;}
        public DateTime DatePublished {get;}

        public List<Review> Reviews { get; }

        public override string ToString()
        {
            return $"{Id} {Name} {Author} {DatePublished.ToLongDateString()}";
        }
    }
}
