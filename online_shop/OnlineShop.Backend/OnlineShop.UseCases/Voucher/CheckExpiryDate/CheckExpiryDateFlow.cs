using OnlineShop.Utils;
using OnlineShop.Core;
using OnlineShop.Core.Interfaces;

namespace OnlineShop.UseCases.Voucher.Crud
{
    public class CheckExpiryDateFlow
    {
        private readonly IVoucher _voucher;
        public CheckExpiryDateFlow(IVoucher voucher)
        {
            _voucher = voucher;
        }
        public Response Execute(string voucherCode)
        {
            var result = _voucher.CheckExpiryDate(voucherCode);
            return new Response(Message.SUCCESS, result);
        }
    }
}
