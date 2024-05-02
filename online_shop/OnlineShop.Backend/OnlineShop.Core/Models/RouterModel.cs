 

namespace OnlineShop.Core.Models
{
    public class RouterModel
    {
        public string Path { get; set; }
        public string Module { get; set; }
        public string Method { get; set; }
        public string ProfileType { get; set; }
        public Delegate Action { get; set; }
    }
}
