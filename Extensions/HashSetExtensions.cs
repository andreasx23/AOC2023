using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class HashSetExtensions
    {
        // https://stackoverflow.com/a/52063869
        public static HashSet<T> FastClone<T>(this HashSet<T> current)
        {
            if (current is null)
            {
                throw new ArgumentNullException(nameof(current));
            }

            return new HashSet<T>(current, current.Comparer);
        }
    }
}
