using OnlineShop.Core.Schemas;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Models;

namespace OnlineShop.Infrastructure.Services
{
    public class VoucherService : IVoucher
    {

        private readonly IDataContext context;
        public VoucherService(IDataContext _ctx)
        {
            context = _ctx;
        }

        public List<VoucherSchema> GetByDiscountAmount(decimal discountAmount)
        {
            List<VoucherSchema> vouchers = context.Vouchers.Where(v => v.DiscountAmount.Equals(discountAmount)).ToList();
            return vouchers;
        }

        public List<VoucherSchema> GetVouchers()
        {
            List<VoucherSchema> vouchers = (
              from v in context.Vouchers
              select v
            ).ToList();

            return vouchers;
        }

        public dynamic ApplyVoucher(int productId, string voucherCode,string freeshipCode)
        {

            var productService = new ProductService(context);
            var product = productService.Get(productId);
            float priceAfterDiscount = 0;

            if (!string.IsNullOrEmpty(voucherCode))
            {
                var voucher = Get(voucherCode);
                if (voucher.DiscountAmount != 0)
                {
                    priceAfterDiscount = product.Price - (float)voucher.DiscountAmount;
                }
                else if (voucher.DiscountPercent != 0)
                {
                    priceAfterDiscount = product.Price * (float)(voucher.DiscountPercent / 100);
                }
            }

            if (priceAfterDiscount <= 0)
            {
                priceAfterDiscount = 0;
            }
            if (!string.IsNullOrEmpty(freeshipCode))
            {
                var freeship = Get(freeshipCode);

                if (freeship != null)
                {
                    product.Status = ProductStatus.FreeShip;
                }
            }


            return new { priceAfterDiscount = priceAfterDiscount, isFreeShip =  ProductStatus.FreeShip };
        }

        public VoucherSchema Get(string voucherCode)
        {
            VoucherSchema voucher = (
              from v in context.Vouchers
              where v.Code == voucherCode
              select v
            ).FirstOrDefault();

            return voucher!;
        }

        public string CheckExpiryDate(string voucherCode)
        {
            VoucherSchema voucher = Get(voucherCode);
            bool isExpired = DateTime.Now > voucher.EndDate;
            var result = isExpired ? "Expired" : "Not expired";
            return result;
        }

        public VoucherSchema DeleteByCode(string voucherCode)
        {
            var voucher = (
              from v in context.Vouchers
              where v.Code == voucherCode
              select v
            ).FirstOrDefault();

            if (voucher == null)
            {
                return voucher;
            }

            context.Vouchers.Remove(voucher);
            context.SaveChanges();
            return voucher;
        }
        public VoucherSchema GetById(int id)
        {
            VoucherSchema voucher = context.Vouchers.FirstOrDefault(x => x.Id == id);

            return voucher;
        }
        public VoucherSchema UpdateById(int id, VoucherSchema updateVoucher)
        {
            VoucherSchema voucher = GetById(id);

            voucher.Code = updateVoucher.Code;
            voucher.Desc = updateVoucher.Desc;
            voucher.DiscountAmount = updateVoucher.DiscountAmount;
            voucher.DiscountPercent = updateVoucher.DiscountPercent;
            voucher.EndDate = updateVoucher.EndDate;
            voucher.Status = updateVoucher.Status;

            context.SaveChanges();

            return voucher;

        }
    }
}
