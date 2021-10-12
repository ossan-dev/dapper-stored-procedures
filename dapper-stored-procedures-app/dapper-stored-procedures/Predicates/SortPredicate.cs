using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dapper_stored_procedures.Predicates
{
    public class SortPredicate
    {
        public string Field { get; set; }
        public string Direction { get; set; }
    }
}
