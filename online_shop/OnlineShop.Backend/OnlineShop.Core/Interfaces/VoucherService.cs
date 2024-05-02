using OnlineShop.Core.Schemas;

namespace OnlineShop.Core.Interfaces
{

    public interface IVoucher
    {
        List<VoucherSchema> GetByDiscountAmount(decimal discountAmount);
        List<VoucherSchema> GetVouchers();
        string CheckExpiryDate(string voucherCode);
        VoucherSchema GetById(int id);
        VoucherSchema Get(string voucherCode);
        VoucherSchema DeleteByCode(string voucherCode);
        dynamic ApplyVoucher(int productId, string voucherCode,string freeshipCode);
        VoucherSchema UpdateById(int Id, VoucherSchema updateVoucher);
    }
}
