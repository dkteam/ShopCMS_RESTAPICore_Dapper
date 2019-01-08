using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCMS_RESTAPICore_Dapper.Models
{
    public class AppRole
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}
