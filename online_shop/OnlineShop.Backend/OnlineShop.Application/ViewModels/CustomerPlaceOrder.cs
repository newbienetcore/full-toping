using AutoMapper;
using OnlineShop.Core.Schemas;
using System.ComponentModel;

namespace OnlineShop.Application.ViewModels
{
    public class CustomerPlaceOrder
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string StreetAddress { get; set; }
        public string? Apartment { get; set; }
        public string TownCity { get; set; }
        public string CountryState { get; set; }
        public string Postcode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? OrderNote { get; set; }
        public int CustomerID { get; set; }
        public float Subtotal { get; set; }
        public float Total { get; set; }

    }
}
