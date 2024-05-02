using AutoMapper;
using OnlineShop.Application.UseCase.Voucher.Crud.Presenter;
using OnlineShop.Application.UseCases.Voucher.Crud.Presenter;
using OnlineShop.Core;
using OnlineShop.Core.Business.GenerateVoucherCode;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Schemas;
using OnlineShop.Infrastructure.Services;
using OnlineShop.UseCases.Voucher.Crud;
using OnlineShop.Utils;

namespace OnlineShop.WebApi.Controllers
{
    public class VoucherCtrl
    {
        private readonly CrudVoucherFlow workflow;
        private readonly CheckExpiryDateFlow checkExpiryDateFlow;
        private readonly ApplyVoucherFlow applyVoucherFlow;
        public VoucherCtrl(IDataContext ctx)
        {
            workflow = new CrudVoucherFlow(ctx, new VoucherService(ctx));
            checkExpiryDateFlow = new CheckExpiryDateFlow(new VoucherService(ctx));
            applyVoucherFlow = new ApplyVoucherFlow(new VoucherService(ctx));
        }
        public async Task<IResult> CreateAsync(IMapper _mapper, CreateVoucherPresenter model)
        {
            if (string.IsNullOrEmpty(model.Code))
            {
                model.Code = GenerateVoucherCode.randomCodeVoucher();
            }
            VoucherSchema voucher = _mapper.Map<VoucherSchema>(model);
            Response response = await workflow.Create(voucher);
            if (response.Status == Message.ERROR)
            {
                return Results.BadRequest();
            }
            return Results.Ok(response);
        }
        public IResult Delete(string vouchercode)
        {
            Response response = workflow.DeleteByCode(vouchercode);
            if (response.Status == Message.ERROR)
            {
                return Results.BadRequest();
            }
            return Results.Ok(response);
        }
        public IResult CheckExpiryDate(string vouchercode)
        {
            Response response = checkExpiryDateFlow.Execute(vouchercode);
            if (response.Status == Message.ERROR)
            {
                return Results.BadRequest();
            }
            return Results.Ok();
        }
        public IResult Update(IMapper _mapper, int id, UpdateVoucherPresenter model)
        {
            VoucherSchema voucher = _mapper.Map<VoucherSchema>(model);
            Response response = workflow.UpdateVoucher(id, voucher);

            if (response.Status == Message.ERROR)
            {
                return Results.BadRequest();
            }

            return Results.Ok(response);

        }
        public IResult GetVoucherById(int id)
        {
            Response response = workflow.GetVoucherById(id);
            if (response.Status == Message.ERROR)
            {
                return Results.BadRequest(response);
            }
            return Results.Ok(response);
        }
        public IResult List(string sortName, string sortType, int cursor, int pageSize)
        {
            Response response = workflow.List();
            List<VoucherSchema> items = (List<VoucherSchema>)response.Result;
            if (sortName == "Id")
            {
                if (sortType == "ASC")
                {
                    items = items.OrderBy(item => item.Id).ToList();
                }
                else if (sortType == "DESC")
                {
                    items = items.OrderByDescending(item => item.Id).ToList();
                }
            }
            ResponsePresenter res = CtrlUtil.ApplyPaging<VoucherSchema, string>(cursor, pageSize, items);

            if (response.Status == Message.ERROR)
            {
                return Results.BadRequest();
            }

            res.Items = CrudVoucherPresenter.PresentList((List<VoucherSchema>)res.Items);

            return Results.Ok(res);
        }
        public IResult ApplyVoucher(int productId, string voucherCode, string? freeshipCode)
        {
            Response response = applyVoucherFlow.Execute(productId, voucherCode, freeshipCode);

            if (response.Status == Message.ERROR)
            {
                return Results.BadRequest();
            }

            return Results.Ok(response);

        }
        public IResult GetVoucherByCode(string code)
        {
            Response response = workflow.Get(code);
            if (response.Status == Message.ERROR)
            {
                return Results.BadRequest();
            }
            return Results.Ok(response);
        }
    }
}
