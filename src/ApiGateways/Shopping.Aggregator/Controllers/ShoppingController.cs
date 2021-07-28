using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
            _orderService = orderService;
        }

        [HttpGet("{userName}", Name = "GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
        {
            //get basket username
            var basket =await _basketService.GetBasket(userName);

            //iterate basket items and consume products with basket item productId member
            foreach (var item in basket.Items) {
                var product = await _catalogService.GetCatalog(item.ProductId);
                // map product related members into basket item dto with extended columns
                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }
            var orders= await _orderService.GetOrdersByUserName(userName);

            // ShoppingModel dto class which including all responses
            var shoppingModel = new ShoppingModel { UserName=userName,
            BasketWithProducts=basket,
            Orders=orders
            };

            return Ok(shoppingModel);
            
        }
    }
}
