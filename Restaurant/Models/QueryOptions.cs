using Microsoft.Extensions.ObjectPool;
using System.Linq.Expressions;

namespace Restaurant.Models
{
    //abstraction class to clean up and centralize querying logic
    //Instead of writing the same filtering/sorting in every controller or service, you can just write it once and pass it around.
    public class QueryOptions<T> where T : class
    {
        public Expression<Func<T, object>>? OrderBy { get; set; }= null!; // Ensure OrderBy is not null
        public Expression<Func<T, bool>>? Where { get; set; } = null!; // Ensure where is not null

        private string[] includes = Array.Empty<string>();

        public string Includes
        {
            
            set => includes = value.Replace(" "," ").Split(','); 
        }
        public string[] GetIncludes() => includes;

        public bool HasWhere => Where !=null;

        public bool HasOrderBy => OrderBy != null;

    }
}