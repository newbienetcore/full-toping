using AutoMapper;
using OnlineShop.Application.UseCase.Voucher.Crud.Presenter;
using OnlineShop.Application.UseCases.Voucher.Crud.Presenter;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Models;
using OnlineShop.WebApi.Controllers;
using static OnlineShop.Utils.CtrlUtil;

namespace OnlineShop.WebApi.Routers
{
    public class VoucherRouter
    {
        private readonly VoucherCtrl voucherCtrl;
        public VoucherRouter(IDataContext ctx)
        {
            voucherCtrl = new VoucherCtrl(ctx);
        }
        public IEnumerable<RouterModel> Get()
        {
            List<RouterModel> routers = new List<RouterModel>();
            var voucherGet = new RouterModel()
            {
                Method = "POST",
                Module = "Voucher",
                Path = "vouchers",
                ProfileType = RoleType.ADMIN_PROFILE,
                Action = async (string sortName = "Id", string sortType = "ASC", int cursor = 0, int pageSize = 10) => voucherCtrl.List(sortName, sortType, cursor, pageSize)
            };
            var GetVoucherById = new RouterModel()
            {
                Method = "GET",
                Module = "Voucher",
                Path = "vouchers/id/{id}",
                ProfileType = RoleType.ADMIN_PROFILE,
                Action = async (int id) => voucherCtrl.GetVoucherById(id)
            };
            var voucherCreate = new RouterModel()
            {
                Method = "POST",
                Module = "Voucher",
                Path = "vouchers/create",
                ProfileType = RoleType.ADMIN_PROFILE,
                Action = async (IMapper _mapper, CreateVoucherPresenter model) => voucherCtrl.CreateAsync(_mapper, model)
            };
            var voucherDeleteByCode = new RouterModel()
            {
                Method = "DELETE",
                Module = "Voucher",
                Path = "vouchers/delete",
                ProfileType = RoleType.ADMIN_PROFILE,
                Action = async (string vouchercode) => voucherCtrl.Delete(vouchercode)
            };
            var voucherUpdate = new RouterModel()
            {
                Method = "PUT",
                Module = "Voucher",
                Path = "vouchers/update/{id}",
                ProfileType = RoleType.ADMIN_PROFILE,
                Action = async (IMapper _mapper, int id, UpdateVoucherPresenter model) => voucherCtrl.Update(_mapper,id,model)
            };
            var voucherApply = new RouterModel()
            {
                Method = "POST",
                Module = "Voucher",
                Path = "vouchers/apply-voucher",
                ProfileType = RoleType.ADMIN_PROFILE,
                Action = async (int productId, string voucherCode, string? freeshipCode) => voucherCtrl.ApplyVoucher(productId, voucherCode, freeshipCode)
            };
            routers.Add(voucherGet);
            routers.Add(GetVoucherById);
            routers.Add(voucherCreate);
            routers.Add(voucherDeleteByCode);
            routers.Add(voucherUpdate);
            routers.Add(voucherApply);
            return routers;
        }
    }
}
