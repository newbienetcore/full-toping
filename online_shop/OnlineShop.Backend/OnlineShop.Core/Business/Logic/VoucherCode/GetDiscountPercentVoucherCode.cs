using System.Text.RegularExpressions;

namespace OnlineShop.Core.Business.GetDiscountPercentVoucherCode
{
  public class GetDiscountPercentVoucherCode
  {
    public static double getDiscountPercentVoucherCode(string specialVoucherCode)
    {
      double numberDiscountPercent = double.Parse(Regex.Match(specialVoucherCode, @"\d+").Value);

      return numberDiscountPercent;
    }
  }
}
