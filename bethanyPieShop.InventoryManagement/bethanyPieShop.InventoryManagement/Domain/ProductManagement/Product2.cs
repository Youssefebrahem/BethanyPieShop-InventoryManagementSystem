namespace bethanyPieShop.InventoryManagement.Domain.ProductManagement
{
    public partial class Product
    {

        public static int StockThreshold = 5;
        public static void ChangeStockThreshold(int newStockThreshold)
        {
            if (newStockThreshold>0)
                StockThreshold = newStockThreshold;
        }
        public void UpdateLowStock()
        {
            if (AmountInStock < StockThreshold)
            {
                IsBelowStockThreshold = true;
            }
        }

        protected void Log(string message)
        {
            //this could be written to a file
            Console.WriteLine(message);
        }

        protected string CreateSimpleProductRepresentation()
        {
            return $"Product {id} ({name})";
        }
    }
}
