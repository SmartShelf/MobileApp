using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShelf.Entities
{
    public class LoginInfo
    {
        //{"firstName":"Fadi","lastName":"Abdin","username":"fadiabdeen","password":"123456"}
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string username { get; set; }

        public string password { get; set; }
    }
    public class Product
    {
        public int id { get; set; }
        public long weight { get; set; }
        public string name { get; set; }
        public string barcode { get; set; }
       
    }

    public class Scale
    {
        public int id { get; set; }

        public long weight { get; set; }

        public int productId { get; set; }

    }

    public class Shelf
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<Scale> scales { get; set; }
    }
    public class ProductList
    {
        public List<Product> Products { get; set; }
    }

    public class Products
    {
        public ProductList productList { get; set; }
    }
}
