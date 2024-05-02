using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Snappier;

namespace Catalog.Infrastructure.Persistence;

public static class CategorySeed
{
    public static async Task SeedCategoryAsync(ApplicationDbContext _context)
    {
        if (await _context.Categories.AnyAsync()) return;

        var categories = new List<Category>()
        {
            new Category
            {
                Name = "Thời Trang Nam",
                Alias = "thoi-trang-nam",
                Description = "Danh mục thời trang nam",
                Level = 1,
                FileName = "images/catalog/54a430ed-fbb0-4761-b246-fd20fc9e1768.png",
                OrderNumber = 1,
                Status = true,
                Path = "/thoi-trang-nam",
                ParentId = null
            },
            new Category
            {
                Name = "Thời Trang Nữ",
                Alias = "thoi-trang-nu",
                Description = "Danh mục thời trang nữ",
                Level = 1,
                FileName = "images/catalog/efc0ae67-28b2-47e0-ac02-c91ce79f47d3.png",
                OrderNumber = 2,
                Status = true,
                Path = "/thoi-trang-nu",
                ParentId = null
            },
            new Category
            {
                Name = "Điện Thoại & Phụ Kiện",
                Alias = "dien-thoai-phu-kien",
                Description = "Danh mục điện thoại và phụ kiện",
                Level = 1,
                FileName = "images/catalog/f4f59825-0800-4357-a7aa-ab235ee3375e.png",
                OrderNumber = 3,
                Status = true,
                Path = "/dien-thoai-phu-kien",
                ParentId = null
            },
            new Category
            {
                Name = "Mẹ & Bé",
                Alias = "me-be",
                Description = "Danh mục mẹ và bé",
                Level = 1,
                FileName = "images/catalog/fbf1cf79-e9c3-433d-aafb-7aeb988d5fbc.png",
                OrderNumber = 4,
                Status = true,
                Path = "/me-be",
                ParentId = null
            },
            new Category
            {
                Name = "Thiết Bị Điện Tử",
                Alias = "thiet-bi-dien-tu",
                Description = "Danh mục thiết bị điện tử",
                Level = 1,
                FileName = "images/catalog/3d70a09d-1eb5-43f1-a69f-14ab39c73092.png",
                OrderNumber = 5,
                Status = true,
                Path = "/thiet-bi-dien-tu",
                ParentId = null
            },
            new Category
            {
                Name = "Nhà Cửa & Đời Sống",
                Alias = "nha-cua-doi-song",
                Description = "Danh mục nhà cửa và đời sống",
                Level = 1,
                FileName = "images/catalog/7d739402-639d-4011-b7e3-f22a227ad37a.png",
                OrderNumber = 6,
                Status = true,
                Path = "/nha-cua-doi-song",
                ParentId = null
            },
            new Category
            {
                Name = "Máy Tính & Laptop",
                Alias = "may-tinh-laptop",
                Description = "Danh mục máy tính và laptop",
                Level = 1,
                FileName = "images/catalog/2a10b1d9-7953-44bf-a5b2-5c6420ea110b.png",
                OrderNumber = 7,
                Status = true,
                Path = "/may-tinh-laptop",
                ParentId = null
            },
            new Category
            {
                Name = "Sắc Đẹp",
                Alias = "sac-dep",
                Description = "Danh mục sắc đẹp",
                Level = 1,
                FileName = "images/catalog/4e6c5e69-4b1b-4d72-8b66-a7001d629b06.png",
                OrderNumber = 8,
                Status = true,
                Path = "/sac-dep",
                ParentId = null
            },
            new Category
            {
                Name = "Máy Ảnh & Máy Quay Phim",
                Alias = "may-anh-may-quay-phim",
                Description = "Danh mục máy ảnh và máy quay phim",
                Level = 1,
                FileName = "images/catalog/9415ae3a-cf33-467a-82ac-fee231eaae07.png",
                OrderNumber = 9,
                Status = true,
                Path = "/may-anh-may-quay-phim",
                ParentId = null
            },
            new Category
            {
                Name = "Sức Khỏe",
                Alias = "suc-khoe",
                Description = "Danh mục sức khỏe",
                Level = 1,
                FileName = "images/catalog/d8efd53f-1d5e-4552-9b0c-623de671825f.png",
                OrderNumber = 10,
                Status = true,
                Path = "/suc-khoe",
                ParentId = null
            },
            new Category
            {
                Name = "Đồng Hồ",
                Alias = "dong-ho",
                Description = "Danh mục đồng hồ",
                Level = 1,
                FileName = "images/catalog/438e5423-993c-434c-b5f4-fdd6180bec21.png",
                OrderNumber = 11,
                Status = true,
                Path = "/dong-ho",
                ParentId = null
            },
            new Category
            {
                Name = "Giày Dép Nữ",
                Alias = "giay-dep-nu",
                Description = "Danh mục giày dép nữ",
                Level = 1,
                FileName = "images/catalog/4854f6e1-b735-4f6f-8937-faecd9478d8e.png",
                OrderNumber = 12,
                Status = true,
                Path = "/giay-dep-nu",
                ParentId = null
            },
            new Category
            {
                Name = "Giày Dép Nam",
                Alias = "giay-dep-nam",
                Description = "Danh mục giày dép nam",
                Level = 1,
                FileName = "images/catalog/7f5fe912-8630-4751-b48a-c9fbfd1f9fae.png",
                OrderNumber = 13,
                Status = true,
                Path = "/giay-dep-nam",
                ParentId = null
            },
            new Category
            {
                Name = "Túi Ví Nữ",
                Alias = "tui-vi-nu",
                Description = "Danh mục túi ví nữ",
                Level = 1,
                FileName = "images/catalog/f2b23b52-b232-47f5-bbbf-da7d83e1ce1a.png",
                OrderNumber = 14,
                Status = true,
                Path = "/tui-vi-nu",
                ParentId = null
            },
            new Category
            {
                Name = "Thiết Bị Điện Gia Dụng",
                Alias = "thiet-bi-dien-gia-dung",
                Description = "Danh mục thiết bị điện gia dụng",
                Level = 1,
                FileName = "images/catalog/60d0a10b-cae6-49a0-8de7-58a7c957d184.png",
                OrderNumber = 15,
                Status = true,
                Path = "/thiet-bi-dien-gia-dung",
                ParentId = null
            },
            new Category
            {
                Name = "Phụ Kiện & Trang Sức Nữ",
                Alias = "phu-kien-trang-suc-nu",
                Description = "Danh mục phụ kiện và trang sức nữ",
                Level = 1,
                FileName = "images/catalog/6f0d11ba-11c1-4704-8039-d8a189694830.png",
                OrderNumber = 16,
                Status = true,
                Path = "/phu-kien-trang-suc-nu",
                ParentId = null
            },
            new Category
            {
                Name = "Thể Thao & Du Lịch",
                Alias = "the-thao-du-lich",
                Description = "Danh mục thể thao và du lịch",
                Level = 1,
                FileName = "images/catalog/e3de60fa-d5dc-432f-925a-0fe3f168957f.png",
                OrderNumber = 17,
                Status = true,
                Path = "/the-thao-du-lich",
                ParentId = null
            },
            new Category
            {
                Name = "Bách Hóa Online",
                Alias = "bach-hoa-online",
                Description = "Danh mục bách hóa trực tuyến",
                Level = 1,
                FileName = "images/catalog/2924460f-bdb9-468f-be95-19077ce5bfe2.png",
                OrderNumber = 18,
                Status = true,
                Path = "/bach-hoa-online",
                ParentId = null
            },
            new Category
            {
                Name = "Ô Tô & Xe Máy & Xe Đạp",
                Alias = "oto-xe-may-xe-dap",
                Description = "Danh mục ô tô, xe máy và xe đạp",
                Level = 1,
                FileName = "images/catalog/e36924a9-a83e-41c0-ade0-a85fb17e4151.png",
                OrderNumber = 19,
                Status = true,
                Path = "/oto-xe-may-xe-dap",
                ParentId = null
            },
            new Category
            {
                Name = "Nhà Sách Online",
                Alias = "nha-sach-online",
                Description = "Danh mục nhà sách trực tuyến",
                Level = 1,
                FileName = "images/catalog/eb102843-df32-44d9-ada7-fe5352ce4b32.png",
                OrderNumber = 20,
                Status = true,
                Path = "/nha-sach-online",
                ParentId = null
            }
        };

        await _context.Categories.AddRangeAsync(categories);
        await _context.SaveChangesAsync();
    }
}