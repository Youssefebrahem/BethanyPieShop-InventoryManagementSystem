using bethanyPieShop.InventoryManagement.Domain.General;
using System.Text;

namespace bethanyPieShop.InventoryManagement.Domain.ProductManagement
{
    public abstract partial class Product: ICloneable
    {

        private int id;
        private string name = string.Empty;
        private string? description;

        protected int maxItemsInStock = 0;


        public int Id
        {
            get { return id; }
            set
            {
                id = value;
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value.Length > 50 ? value[..50] : value;
            }
        }
        public string? Description
        {
            get { return description; }
            set
            {
                if (value == null)
                    description = string.Empty;
                else
                {
                    description = value.Length > 250 ? value[..250] : value;
                }
            }
        }

        public UnitType UnitType { get; set; }
        public int AmountInStock { get; protected set; }
        public bool IsBelowStockThreshold { get; protected set; }
        public Price Price { get;set; }


        public Product(int id) : this(id, string.Empty)
        { }   // ': this' Syntax: Used for constructor chaining within the same class.
        public Product(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public Product(int id, string name, string? description,Price price, UnitType unitType, int maxAmountInStock)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            UnitType = unitType;

            maxItemsInStock = maxAmountInStock;

            UpdateLowStock();
        }

        public virtual void UseProduct(int items)
        {
            if (items <= AmountInStock)
            {
                AmountInStock -= items; //update amount

                UpdateLowStock(); //check alert

                Log($"Amount in stock update. Now {AmountInStock} items in stock."); //Console.WriteLine("Message");
            }
            else
            {
                Log($"Not enough items on stock for {CreateSimpleProductRepresentation()}. {AmountInStock} available but {items} requested.");
            }
        }

        //public virtual void IncreasStock()
        //{
        //    AmountInStock++;
        //}

        public abstract void IncreasStock();
        public virtual void IncreasStock(int amount)
        {
            int newAmount = AmountInStock + amount;
            if (newAmount <= maxItemsInStock)
            {
                AmountInStock += amount;
            }
            else
            {
                AmountInStock = maxItemsInStock; //store posible items.. oythers isn't stored
                Log($"{CreateSimpleProductRepresentation} stock overflow. {newAmount - AmountInStock} item(s) ordered that couldn't be stored.");
            }
            UpdateLowStock();
        }
        protected virtual void DecreasStock(int items, string reason)
        {
            if (items <= AmountInStock)
            {
                AmountInStock -= items;
            }
            else
            {
                AmountInStock = 0;
            }

            UpdateLowStock();
            Log(reason);
        }

        public virtual string DisplayDetailsShort()
        {
            return $"{id}. {name} \n{AmountInStock} items in stock";
        }

        public virtual string DisplayDetailsFull()
        { 
            return DisplayDetailsFull("");
        }
        public virtual string DisplayDetailsFull(string extraDetails)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{Id}. {Name} \n{Description}\n{Price}\n{AmountInStock} item(s) in stock.");

            sb.Append(extraDetails);

            if (IsBelowStockThreshold)
            {
                sb.Append("\n!!STOCK LOW!!");
            }

            return sb.ToString();
        }
        protected virtual double GetProductStockValue()
        {
            return Price.ItemPrice * AmountInStock;
        }

        public abstract object Clone();

    }
}
