namespace Shopping.Web.Pages
{
    public class IndexModel (ICatalogService catalogService, IBasketService basketService, ILogger<IndexModel> logger)
        : PageModel
    {
        public IEnumerable<ProductModel> ProductList { get; set; } = new List<ProductModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            logger.LogInformation("Index page visited.");
            var result = await catalogService.GetProducts();
            ProductList = result.Products;

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
        {
            logger.LogInformation("Add to cart");
            var product = await catalogService.GetProduct(productId);

            var basket = await basketService.LoadUserBasket();
            basket.Items.Add(new ShoppingCartItemModel
            {
                ProductId = productId,
                ProductName = product.Product.Name,
                Price = product.Product.Price,
                Quantity = 1,
                Color = "Black"
            });

            await  basketService.StoreBasket(new StoreBasketRequest(basket));
            return RedirectToPage("Cart");
        }
    }
}