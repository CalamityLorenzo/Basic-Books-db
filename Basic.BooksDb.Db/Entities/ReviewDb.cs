﻿using Basic.BooksDb.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BooksDb.Entities
{
    public class ReviewDb : AuditBaseDb, IEntityDbId<Guid>
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public BookDb Book { get; set; }
        public string Name { get; set; }
        public short Score { get; set; }
    }
}
