using bethanyPieShop.InventoryManagement.Domain.General;

namespace bethanyPieShop.InventoryManagement.Domain.ProductManagement
{
    static class ProductExtensions
    {
        static double dollarToEuro = 0.92;
        static double euroToDollar = 1.11;

        static double poundToEuro = 1.14;
        static double euroToPound = 0.88;

        static double dollarToPound = 0.81;
        static double poundToDollar = 1.14;

        public static double ConvertProductPrice(this Product product, Currency targetCurrency)
        {
            Currency sourceCurrency = product.Price.Currency;
            double originalPrice = product.Price.ItemPrice;
            double convertedPrice = 0.0;

            if (sourceCurrency == Currency.dollar && targetCurrency == Currency.Euro)
            {
                convertedPrice = originalPrice * dollarToEuro;
            }
            else if (sourceCurrency == Currency.Euro && targetCurrency == Currency.dollar)
            {
                convertedPrice = originalPrice * euroToDollar;
            }
            else if (sourceCurrency == Currency.pound && targetCurrency == Currency.Euro)
            {
                convertedPrice = originalPrice * poundToEuro;
            }
            else if (sourceCurrency == Currency.Euro && targetCurrency == Currency.pound)
            {
                convertedPrice = originalPrice * euroToPound;
            }
            else if (sourceCurrency == Currency.dollar && targetCurrency == Currency.pound)
            {
                convertedPrice = originalPrice * dollarToPound;
            }
            else if (sourceCurrency == Currency.pound && targetCurrency == Currency.dollar)
            {
                convertedPrice = originalPrice * poundToDollar;
            }
            else
            {
                convertedPrice = originalPrice;
            }

            product.Price.ItemPrice = convertedPrice;

            return convertedPrice;
        }
    }
}
