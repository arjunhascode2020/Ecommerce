namespace Basket.Application.Responses
{
    public class ShoppingCartResponse
    {
        public string UserName { get; set; }

        public List<ShoppingCartItemResponse> ShoppingCartItems { get; set; }
        public ShoppingCartResponse()
        {
            
        }
        public ShoppingCartResponse(string userName)
        {
            UserName = userName;
        }

        public decimal TotalPrice
        {
            get
            {
                decimal total = 0;
                if (ShoppingCartItems != null)
                {
                    foreach (var item in ShoppingCartItems)
                    {
                        total += item.Price * item.Quantity;
                    }
                }
                return total;
            }
        }
    }
}
