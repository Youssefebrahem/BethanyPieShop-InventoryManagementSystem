using bethanyPieShop.InventoryManagement.Domain.Contracts;
using bethanyPieShop.InventoryManagement.Domain.General;
using bethanyPieShop.InventoryManagement.Domain.ProductManagement;
using System.Text;

namespace bethanyPieShop.InventoryManagement
{
    internal class ProductRepository
    {
        // private string directory = @"D:\data\BethanyPieShop";
        // private string productsFileName = "product.txt";
        // private string productsSaveFileName = "products2.txt";

        private void checkForExeistingProductFile()
        {
            string path = @"..\PieRepoData.txt";
            bool existingFileFound = File.Exists(path);
            if (!existingFileFound)
            {
                //create directory
                //if (Directory.Exists(path))
                //    Directory.CreateDirectory(directory);

                //create an empty file
                //using FileStream fs = File.Create(path);
                Console.WriteLine("==================================!!NOT FOUND!!=============================");
            }
        }
        public List<Product> LoadProductFromFile()
        {
            List<Product> products = new List<Product>();

            string path = @"..\PieRepoData.txt";
            try
            {
                checkForExeistingProductFile();
                string[] productsAsString = File.ReadAllLines(path);
                for (int i = 0; i < productsAsString.Length; i++)
                {
                    string[] productSplit = productsAsString[i].Split(';');;

                    //1- ID
                    bool success = int.TryParse(productSplit[0], out int productId);
                    if (!success)
                    {
                        productId = 0;
                    }

                    //2- Name
                    string name = productSplit[1];

                    //3- Description
                    string description = productSplit[2];

                    //4- Max item in stock
                    success = int.TryParse(productSplit[3], out int maxItemInStock);
                    if (!success)
                    {
                        maxItemInStock = 100; //default value
                    }
                    //5- Item price
                    success = int.TryParse(productSplit[4], out int itemPrice);
                    if (!success)
                    {
                        itemPrice = 0;
                    }

                    //6- Currency
                    success = Enum.TryParse(productSplit[5], out Currency currency);
                    if (!success)
                    {
                        currency = Currency.dollar;
                    }

                    //7- Unit type
                    success = Enum.TryParse(productSplit[6], out UnitType unitType);
                    if (!success)
                    {
                        unitType = UnitType.PerItem;
                    }

                    string productType = productSplit[7];

                    Product product = null;

                    switch (productType)
                    {
                        case "1":
                            success = int.TryParse(productSplit[8], out int amountPerBox);
                            if (!success)
                            {
                                amountPerBox = 1;//default value
                            }

                            product = new BoxedProduct(productId, name, description, new Price() { ItemPrice = itemPrice, Currency = currency }, maxItemInStock, amountPerBox);
                            break;

                        case "2":
                            product = new FreshProduct(productId, name, description, new Price() { ItemPrice = itemPrice, Currency = currency }, unitType, maxItemInStock);
                            break;
                        case "3":
                            product = new BulkProduct(productId, name, description, new Price() { ItemPrice = itemPrice, Currency = currency }, maxItemInStock);
                            break;
                        case "4":
                            product = new RegularProduct(productId, name, description, new Price() { ItemPrice = itemPrice, Currency = currency }, unitType, maxItemInStock);
                            break;
                    }

                    //Product product = new Product(productId, name, description, new Price() { ItemPrice = itemPrice, Currency = currency}, unitType, maxItemInStock);
                    
                    products.Add(product);
                }
            }
            catch (IndexOutOfRangeException iex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong parsing the file, please check the data!");
                Console.WriteLine(iex.Message);
            }
            catch (FileNotFoundException fnfex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The file couldn't be found!");
                Console.WriteLine(fnfex.Message);
                Console.WriteLine(fnfex.StackTrace);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong while loading the file!");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ResetColor();
            }

            return products;
        }

        public void SaveToFile(List<ISaveable> saveables)
        {
            StringBuilder sb = new StringBuilder();
            string path = @"..\PieRepoData.txt";

            foreach (var item in saveables)
            {
                sb.Append(item.ConvertToStringForSaving());
                sb.Append(Environment.NewLine);
            }

            File.WriteAllText(path, sb.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Saved items successfully");
            Console.ResetColor();
        }
    }
}
