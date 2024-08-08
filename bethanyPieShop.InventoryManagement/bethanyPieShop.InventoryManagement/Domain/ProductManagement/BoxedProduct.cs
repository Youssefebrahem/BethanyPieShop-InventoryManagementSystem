using bethanyPieShop.InventoryManagement.Domain.General;
using bethanyPieShop.InventoryManagement.Domain.Contracts;
using System.Text;

namespace bethanyPieShop.InventoryManagement.Domain.ProductManagement
{
    public sealed class BoxedProduct : Product, ISaveable, ILoggable
    //sealed keyword => means that no other class can inherit from this class.
    //This is useful when you want to prevent inheritance for security reasons,
    //to improve performance, or to avoid the complexity of supporting inheritance.
    {
        private int amountPerBox;
        public int AmountPerBox
        {
            get{ return amountPerBox; }
            set
            {
                amountPerBox = value;
            }
        }

        public override string DisplayDetailsFull()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Boxed Product\n");

            sb.Append($"{Id} {Name}\n{Description}\n{Price}\n{AmountInStock} item(s) in stock");

            if (IsBelowStockThreshold)
            {
                sb.Append("\nSTOCK LOW!!");
            }
            return sb.ToString();
        }
        public override void UseProduct(int items)
        {
            int smallesMultible = 0;
            int batchSize;

            while (true)
            {
                smallesMultible++;
                if (smallesMultible * amountPerBox > items)
                {
                    batchSize = smallesMultible * amountPerBox;
                    break;
                }
            }
            
            base.UseProduct(batchSize);
        }
        public override void IncreasStock()
        {
            IncreasStock(1);
            //AmountInStock += amountPerBox;
        }
        public override void IncreasStock(int amount)
        {
            int newAmount = AmountInStock + amount * AmountPerBox;

            if (newAmount <= maxItemsInStock)
            {
                AmountInStock += amount * AmountPerBox;
            }
            else
            {
                AmountInStock = maxItemsInStock; //store posible items.. oythers isn't stored
                Log($"{CreateSimpleProductRepresentation} stock overflow. {newAmount - AmountInStock} item(s) ordered that couldn't be stored.");
            }
            if (AmountInStock > StockThreshold)
            {
                IsBelowStockThreshold = false;
            }
        }
        public string ConvertToStringForSaving()
        {
            return $"{Id};{Name};{Description};{maxItemsInStock};{Price.ItemPrice};{(int)Price.Currency};{(int)UnitType};1;{AmountPerBox};";
        }

        public override object Clone()
        {
            return new BoxedProduct(0, this.Name, this.Description, new Price() { ItemPrice = this.Price.ItemPrice, Currency = this.Price.Currency }, this.maxItemsInStock, this.AmountPerBox);
        }

        void ILoggable.Log(string message)
        {}

        public BoxedProduct(int id, string name, string? description, Price price, int maxAmountInStock, int amountPerBox) : base(id, name, description, price, UnitType.PerBox, maxAmountInStock)
        {
            AmountPerBox = amountPerBox;
        }
    }
}
