using dapper_stored_procedures.Predicates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dapper_stored_procedures.ApiModels
{
    public class ComplexPaging
    {
        public ComplexPaging()
        {
            FilterPredicates = new HashSet<FilterPredicate>();
            SortPredicates = new HashSet<SortPredicate>();
        }

        public int PageNumber { get; set; }
        public int ItemsPerPage { get; set; }
        public IEnumerable<FilterPredicate> FilterPredicates { get; set; }
        public IEnumerable<SortPredicate> SortPredicates { get; set; }
    }
}
