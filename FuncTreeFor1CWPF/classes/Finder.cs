using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1CWPF.classes
{
    public class Finder
    {
        const char methodPrefix = 'м';
        const char exportPrefix = 'э';

        public static IOrderedEnumerable<FinderItem> Find(FinderList finderList, string filter)
        {

            if (filter.Trim().Length <= 2) 
                return finderList.OrderBy(x => x.Name);

            var method = false;
            var export = false;
            var filterName = "";

            var mFilters = filter.Trim().ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in mFilters)
            {
                if (item.Length == 1)
                {
                    var command = item[0];

                    if (command == methodPrefix)
                        method = true;

                    if (command == exportPrefix)
                        export = true;
                }
                else
                {
                    filterName = item;
                }
            }

            if (filterName.Length <= 1)
                new Exception("Не верно задана строка фильтра. Заданы команды, но не задана строка поиска.");

            var result = finderList
                .Where(x =>
                    (
                        (filterName.Length == 0) 
                        || (filterName.Length > 0 && x.Name.IndexOf(filterName, StringComparison.OrdinalIgnoreCase) != -1)
                    )
                    && (
                        export ? x.Export : true
                    )
                    && (
                        method ? x.Object is FunctionInfo : true
                    )
                )
                .OrderBy(x => x.Name);

            return result;
        }
    }
}
