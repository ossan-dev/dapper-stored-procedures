using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace dapper_stored_procedures.Utils
{
    public static class ExpandoObjectUtils
    {
        // https://www.oreilly.com/content/building-c-objects-dynamically/
        public static void AddProperty(ExpandoObject obj, string key, object value)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = obj as IDictionary<string, object>;
            if (expandoDict.ContainsKey(key))
                expandoDict[key] = value;
            else
                expandoDict.Add(key, value);
        }
    }
}
