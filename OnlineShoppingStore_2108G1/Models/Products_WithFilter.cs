using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShoppingStore_2108G1.Models
{
    public class Products_WithFilter
    {
        public PagedList.IPagedList<Product> ProductList { get; set; }
    }
    public class ProductsList
    {
        public PagedList.IPagedList<Product> ProductList { get; set; }
    }

}