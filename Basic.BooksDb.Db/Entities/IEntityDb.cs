using System;
using System.Collections.Generic;
using System.Text;

namespace Basic.BooksDb.Entities
{
    interface IEntityDbId<T>
    {
        T Id { get; }
    }
}
