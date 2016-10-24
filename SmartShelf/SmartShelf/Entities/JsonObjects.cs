using System;
using System.Collections;
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
        public string id { get; set; }
        public string weight { get; set; }

        public string packageWeight { get; set; }
        public string name { get; set; }
        public string barcode { get; set; }
        
        

    }

    public class Scale
    {
        public int id { get; set; }

        public string weight { get; set; }

        public string productId { get; set; }

        public string persentage { get; set; }

        public string registerDate { get; set; }

        public string updateDate { get; set; }

        public string estimatedDate { get; set; }
        public Scale()
        {

        }

    }

    public class Shelf
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<Scale> scales { get; set; }
        public Shelf()
        {

        }
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
