using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ShopCMS_RESTAPICore_Dapper.Dtos;
using ShopCMS_RESTAPICore_Dapper.Extensions;
using ShopCMS_RESTAPICore_Dapper.Filters;
using ShopCMS_RESTAPICore_Dapper.Models;
using ShopCMS_RESTAPICore_Dapper.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCMS_RESTAPICore_Dapper.Controllers
{
    [Route("api/{culture}/[controller]")]
    [ApiController]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class ProductController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly ILogger<ProductController> _logger;
        private readonly IStringLocalizer<ProductController> _localizer;
        private readonly LocService _locService;

        public ProductController(IConfiguration configuration, ILogger<ProductController> logger, IStringLocalizer<ProductController> localizer, LocService locService)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
            _logger = logger;
            _localizer = localizer;
            _locService = locService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            var culture = CultureInfo.CurrentCulture.Name;
            //string text = _localizer["Test"];
            //string text1 = _locService.GetLocalizedHtmlString("ForgotPassword");

            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@language", culture);

                var result = await conn.QueryAsync<Product>("Get_Product_All", parameters, null, null, CommandType.StoredProcedure);

                return result;
            }
        }

        [HttpGet("paging",Name = "GetPaging")]
        public async Task<PagedResult<Product>> GetPaging(string keyword, int categoryId, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@keyword", keyword);
                parameters.Add("@categoryId", categoryId);
                parameters.Add("@pageIndex", pageIndex);
                parameters.Add("@pageSize", pageSize);
                parameters.Add("@language", CultureInfo.CurrentCulture.Name);
                parameters.Add("@totalRow", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var result = await conn.QueryAsync<Product>("Get_Product_AllPaging", parameters, null, null, CommandType.StoredProcedure);

                int totalRow = parameters.Get<int>("@totalRow");

                var pageResult = new PagedResult<Product>()
                {
                    Items = result.ToList(),
                    TotalRow = totalRow,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                return pageResult;
            }
        }

        // GET: api/Product/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<Product> Get(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@id", id);
                parameters.Add("@language", CultureInfo.CurrentCulture.Name);

                var result = await conn.QueryAsync<Product>("Get_Product_ById", parameters, null, null, CommandType.StoredProcedure);

                return result.Single();
            }
        }

        // POST: api/Product
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            int newId = 0;
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@name", product.Name);
                parameters.Add("@description", product.Description);
                parameters.Add("@content", product.Content);
                parameters.Add("@seoTitle", product.SeoTitle);
                parameters.Add("@seoAlias", product.SeoAlias);
                parameters.Add("@seoKeyword", product.SeoKeyword);
                parameters.Add("@seoDescription", product.SeoDescription);

                parameters.Add("@sku", product.Sku);
                parameters.Add("@thumnailImage", product.ThumnailImage);
                parameters.Add("@imageUrl", product.ImageUrl);
                parameters.Add("@imageList", product.ImageList);
                parameters.Add("@price", product.Price);
                parameters.Add("@promotionPrice", product.PromotionPrice);
                parameters.Add("@viewCount", product.ViewCount);
                parameters.Add("@isActive", product.IsActive);
                parameters.Add("@language", CultureInfo.CurrentCulture.Name);
                parameters.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                parameters.Add("@categoryIdS", product.CategoryIds);

                var result = await conn.ExecuteAsync("Create_Product", parameters, null, null, CommandType.StoredProcedure);

                newId = parameters.Get<int>("@id");

                return Ok(newId);
            }
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Put(int id, [FromBody] Product product)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@id", product.Id);

                parameters.Add("@name", product.Name);
                parameters.Add("@description", product.Description);
                parameters.Add("@content", product.Content);
                parameters.Add("@seoTitle", product.SeoTitle);
                parameters.Add("@seoAlias", product.SeoAlias);
                parameters.Add("@seoKeyword", product.SeoKeyword);
                parameters.Add("@seoDescription", product.SeoDescription);

                parameters.Add("@sku", product.Sku);
                parameters.Add("@thumnailImage", product.ThumnailImage);
                parameters.Add("@imageUrl", product.ImageUrl);
                parameters.Add("@imageList", product.ImageList);
                parameters.Add("@price", product.Price);
                parameters.Add("@promotionPrice", product.PromotionPrice);
                parameters.Add("@viewCount", product.ViewCount);
                parameters.Add("@isActive", product.IsActive);
                parameters.Add("@rateTotal", product.RateTotal);
                parameters.Add("@rateCount", product.RateCount);
                parameters.Add("@language", CultureInfo.CurrentCulture.Name);

                parameters.Add("@categoryIds", product.CategoryIds);

                await conn.ExecuteAsync("Update_Product", parameters, null, null, CommandType.StoredProcedure);

                return Ok();
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@id", id);

                await conn.ExecuteAsync("Delete_Product_ById", parameters, null, null, CommandType.StoredProcedure);
            }
        }
    }
}