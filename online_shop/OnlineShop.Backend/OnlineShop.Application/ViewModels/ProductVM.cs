using AutoMapper;
using Newtonsoft.Json;
using OnlineShop.Core.Schemas;

namespace OnlineShop.Application.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public float Price { get; set; }
        public float Discount { get; set; }
        public float PriceAfterDiscount { get; set; }
        public string DefaultImage { get; set; }
        public List<string> Images { get; set; }
        public List<string> Sizes { get; set; }
        public List<string> Colors { get; set; }

    }
}
