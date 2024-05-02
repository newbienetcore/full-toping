using OnlineShop.Utils;
using OnlineShop.Core.Schemas;
using OnlineShop.Core;
using OnlineShop.Core.Interfaces;

namespace OnlineShop.UseCases.Voucher.Crud
{
    public class CrudVoucherFlow
    {
        private readonly IDataContext dbContext;
        private readonly IVoucher _voucher;
        public CrudVoucherFlow(IDataContext ctx, IVoucher voucher)
        {
            dbContext = ctx;
            _voucher = voucher;
        }

        public async Task<Response> Create(VoucherSchema voucher)
        {
            var result = dbContext.Vouchers.Add(voucher);
            return new Response(Message.SUCCESS, result);
        }

        public async Task<Response> Update(VoucherSchema voucher)
        {
            var result = dbContext.Vouchers.Update(voucher);
            return new Response(Message.SUCCESS, result);
        }


        public Response List()
        {
            var vouchers = dbContext.Vouchers.ToList();
            return new Response(Message.SUCCESS, vouchers);
        }

        public Response Delete(int id)
        {
            var voucher = dbContext.Vouchers.Find(id);
            var result = dbContext.Vouchers.Remove(voucher);
            return new Response(Message.SUCCESS, result);
        }

        public Response Get(string voucherCode)
        {
            var result = dbContext.Vouchers.Where(x => x.Code == voucherCode).FirstOrDefault();
            return new Response(Message.SUCCESS, result);
        }

        public Response GetVoucherById(int id)
        {
            var result = _voucher.GetById(id);
            return new Response(Message.SUCCESS, result);
        }
        public Response UpdateVoucher(int Id, VoucherSchema voucher) 
        {
            var result = _voucher.UpdateById(Id, voucher);
            return new Response(Message.SUCCESS, result);

        }
        //public Response CheckExpiryDate(string voucherCode)
        //{
        //    var result = dbContext.Vouchers.CheckExpiryDate(voucherCode);
        //    return new Response(Message.SUCCESS, result);
        //}

        //public Response ApplyVoucher(int productId, string voucherCode, string freeshipCode = null)
        //{
        //    var result = dbContext.Vouchers.ApplyVoucher(productId, voucherCode, freeshipCode);
        //    return new Response(Message.SUCCESS, result);
        //}

        public Response DeleteByCode(string voucherCode)
        {
            var voucher = dbContext.Vouchers.Where(x => x.Code == voucherCode).FirstOrDefault();
            var result = dbContext.Vouchers.Remove(voucher);
            return new Response(Message.SUCCESS, result);
        }



        // public Response Deletes(int[] ids)
        // {
        //   var result = uow.Vouchers.Deletes(ids);
        //   return new Response(Message.SUCCESS, result);
        // }
    }
}
