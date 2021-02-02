using System;
using System.Collections.Generic;
using System.Text;

namespace WeBuy.Model.Common
{
    public class QueryModelBase
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
