namespace Basket.Core.Entities
{
    public class BasketCheckout
    {
        public string UserName { get; set; } = string.Empty; // UserName is the unique identifier for the user
        public decimal TotalPrice { get; set; } // Total price of the items in the basket

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        public string CardName { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string Expiration { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;
        public int PaymentMethod { get; set; } // 0 - Credit Card, 1 - Paypal
    }
}
