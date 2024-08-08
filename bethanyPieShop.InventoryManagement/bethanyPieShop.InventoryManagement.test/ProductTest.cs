using bethanyPieShop.InventoryManagement.Domain.General;
using bethanyPieShop.InventoryManagement.Domain.ProductManagement;

namespace bethanyPieShop.InventoryManagement.Tests
{
    public class ProductTests
    {
        [Fact]
        public void UseProduct_Reduces_AmountInStock()
        {
            //Arrange
            Product product = new RegularProduct(1, "Sugar", "Lorem ipsum", new Price() { ItemPrice = 10, Currency = Currency.Euro }, UnitType.PerKg, 100);

            product.IncreasStock(100);

            //Act
            product.UseProduct(20);

            //Assert
            Assert.Equal(80, product.AmountInStock);

        }

        [Fact]
        public void UseProduct_Reduces_AmountInStock_StockBelowTreshold()
        {
            //Arrange
            Product product = new RegularProduct(1, "Sugar", "Lorem ipsum", new Price() { ItemPrice = 10, Currency = Currency.Euro }, UnitType.PerKg, 100);

            int increaseValue = 100;
            product.IncreasStock(increaseValue);

            //Act
            product.UseProduct(increaseValue - 1);

            //Assert
            Assert.True(product.IsBelowStockThreshold);
        }

        [Fac    t]
        public void IncreaseStock_AddsOne()
        {
            //Arrange
            Product product = new RegularProduct(1, "Sugar", "Lorem ipsum", new Price() { ItemPrice = 10, Currency = Currency.Euro }, UnitType.PerKg, 100);

            //Act
            product.IncreasStock();

            //Assert
            Assert.Equal(1, product.AmountInStock);
        }

        [Fact]
        public void IncreaseStock_AddsPassedInValue_BelowMaxAmount()
        {
            //Arrange
            Product product = new RegularProduct(1, "Sugar", "Lorem ipsum", new Price() { ItemPrice = 10, Currency = Currency.Euro }, UnitType.PerKg, 100);

            //Act
            product.IncreasStock(20);

            //Assert
            Assert.Equal(20, product.AmountInStock);
        }

        [Fact]
        public void IncreaseStock_AddsPassedInValue_AboveMaxAmount()
        {
            //Arrange
            Product product = new RegularProduct(1, "Sugar", "Lorem ipsum", new Price() { ItemPrice = 10, Currency = Currency.Euro }, UnitType.PerKg, 100);

            //Act
            product.IncreasStock(300);

            //Assert
            Assert.Equal(100, product.AmountInStock);
        }
    }
}