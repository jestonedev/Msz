using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.Options
{
    public class PageOptions
    {
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }

        public PageOptions()
        {
            PageSize = 10;
            PageIndex = 0;
        }
    }
}
