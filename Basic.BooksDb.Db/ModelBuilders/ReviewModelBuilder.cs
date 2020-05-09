using Microsoft.EntityFrameworkCore;
using BooksDb.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooksDb.ModelBuilders 
{
    class ReviewModelBuilder : IEntityTypeConfiguration<ReviewDb>
    {
        public void Configure(EntityTypeBuilder<ReviewDb> builder)
        {
            builder.Property(p => p.Id).HasDefaultValueSql("NEWID()");
            builder.Property(p => p.Name).IsRequired();
        }
    }
}
