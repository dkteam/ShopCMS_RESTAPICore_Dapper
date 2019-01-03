using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCMS_RESTAPICore_Dapper.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Sku { get; set; }

        public string ImageUrl { get; set; }

        public string ImageList { get; set; }

        public float? Price { get; set; }

        public float? PromotionPrice { get; set; }

        public int? ViewCount { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public int? RateTotal { get; set; }

        public int? RateCount { get; set; }
    }
}
