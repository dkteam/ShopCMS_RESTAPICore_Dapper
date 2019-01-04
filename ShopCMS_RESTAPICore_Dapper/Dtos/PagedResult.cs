using ShopCMS_RESTAPICore_Dapper.Models;
using System.Collections.Generic;

namespace ShopCMS_RESTAPICore_Dapper.Dtos
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRow { get; set; }
    }
}