using System;
using System.Collections.Generic;
using System.Text;

namespace Basic.BooksDb.Entities
{
    public class AuditBaseDb
    {
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Created { get; set; }
    }
}
