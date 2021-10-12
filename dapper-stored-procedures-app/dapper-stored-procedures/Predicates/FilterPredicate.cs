using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dapper_stored_procedures.Predicates
{
    public class FilterPredicate
    {
        public string LogicalOperator { get; set; }
        public string Field { get; set; }
        public string RelationalOperator { get; set; }
        public string LiteralToCompare { get; set; }

    }
}
