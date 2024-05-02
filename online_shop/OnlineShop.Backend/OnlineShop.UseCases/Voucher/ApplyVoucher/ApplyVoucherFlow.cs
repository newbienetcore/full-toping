using OnlineShop.Utils;
using OnlineShop.Core;
using OnlineShop.Core.Interfaces;

namespace OnlineShop.UseCases.Voucher.Crud
{
    public class ApplyVoucherFlow
    {
        private readonly IVoucher _voucher;
        public ApplyVoucherFlow(IVoucher voucher)
        {
            _voucher = voucher;
        }

        public Response Execute(int productId, string voucherCode, string freeshipCode = null)
        {
            var result = _voucher.ApplyVoucher(productId, voucherCode, freeshipCode);
            return new Response(Message.SUCCESS, result);
        }

    }
}
