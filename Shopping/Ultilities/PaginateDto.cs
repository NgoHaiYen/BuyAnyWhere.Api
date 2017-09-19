using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping.Ultilities
{
    public class PaginateDto
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}