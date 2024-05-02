using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence;

public static class SupplierSeed
{
    public static async Task SeedSupplierAsync(ApplicationDbContext _context)
    {
        if (await _context.Suppliers.AnyAsync()) return;

        var suppliers = new List<Supplier>
        {
            new Supplier()
            {
                Name = "Apple",
                Alias = "apple",
                Description = "Nhà cung cấp điện thoại Apple",
                Delegate = "Đỗ Chí Hùng",
                Email = "dohung.csharp@gmail.com",
                Bank = "MB Bank",
                AccountNumber = "0976580418",
                BankAddress = "Tầng 1 - Toà Nhà 17T2 - Đ.Hoàng Đạo Thuý - Trung Hoà - Cầu Giấy - Hà Nội",
                AddressOne = "Đông Kết, Khoái Châu, Hưng Yên",
                AddressTwo = "301 Kim Mã, Ba Đình, Hà Nội",
                Phone = "0976580418",
                Fax = "833000",
                NationCode = "0084",
                ProvinceCode = "01",
                DistrictCode = "001"
            },
            new Supplier()
            {
                Name = "Sam sung",
                Alias = "sam-sung",
                Description = "Nhà cung cấp điện thoại Samsung",
                Delegate = "Kim Jong-un",
                Email = "dohung.csharp@gmail.com",
                Bank = "Vietcombank",
                AccountNumber = "1234567890",
                BankAddress = "Tầng 2 - Toà Nhà 20T3 - Phố Lê Văn Lương - Thanh Xuân - Hà Nội",
                AddressOne = "Số 10, Đường Số 5, Quận 7, TP. Hồ Chí Minh",
                AddressTwo = "45 Đinh Tiên Hoàng, Quận 1, TP. Hồ Chí Minh",
                Phone = "0987654321",
                Fax = "833001",
                NationCode = "0084",
                ProvinceCode = "02",
                DistrictCode = "002"
            },
            new Supplier()
            {
                Name = "LG",
                Alias = "lg",
                Description = "Nhà cung cấp thiết bị điện tử LG",
                Delegate = "Park Jung-min", 
                Email = "dohung.csharp@gmail.com",
                Bank = "Shinhan Bank",
                AccountNumber = "0987654321",
                BankAddress = "Tầng 3 - Toà Nhà 15T5 - Đường Phạm Hùng - Mỹ Đình - Nam Từ Liêm - Hà Nội",
                AddressOne = "Số 25, Đường Số 6, Quận 9, TP. Hồ Chí Minh",
                AddressTwo = "89 Lý Thường Kiệt, Quận Hoàn Kiếm, Hà Nội",
                Phone = "0933334444",
                Fax = "833002",
                NationCode = "0084",
                ProvinceCode = "03",
                DistrictCode = "003"
            },
            // Thêm nhà cung cấp thứ tư
            new Supplier()
            {
                Name = "Xiaomi",
                Alias = "xiaomi",
                Description = "Nhà cung cấp thiết bị điện tử Xiaomi",
                Delegate = "Lei Jun",
                Email = "dohung.csharp@gmail.com",
                Bank = "Techcombank",
                AccountNumber = "2009876543",
                BankAddress = "Tầng 4 - Toà Nhà 25T4 - Đường Nguyễn Chí Thanh - Đống Đa - Hà Nội",
                AddressOne = "Lô D, Khu công nghệ cao, Quận 9, TP. Hồ Chí Minh",
                AddressTwo = "100 Bà Triệu, Quận Hai Bà Trưng, Hà Nội",
                Phone = "0945678901",
                Fax = "833003",
                NationCode = "0084",
                ProvinceCode = "04",
                DistrictCode = "004"
            },
            new Supplier()
            {
                Name = "Huawei",
                Alias = "huawei",
                Description = "Nhà cung cấp thiết bị mạng và điện thoại Huawei",
                Delegate = "Ren Zhengfei",
                Email = "dohung.csharp@gmail.com",
                Bank = "HSBC Bank",
                AccountNumber = "1122334455",
                BankAddress = "Tầng 5 - Toà Nhà 30T5 - Đường Trần Duy Hưng - Cầu Giấy - Hà Nội",
                AddressOne = "Khu công nghệ cao, Quận Thanh Xuân, Hà Nội",
                AddressTwo = "22 Lê Lợi, Quận 1, TP. Hồ Chí Minh",
                Phone = "0912345678",
                Fax = "833004",
                NationCode = "0084",
                ProvinceCode = "05",
                DistrictCode = "005"
            },
            new Supplier()
            {
                Name = "Sony",
                Alias = "sony",
                Description = "Nhà cung cấp thiết bị điện tử Sony",
                Delegate = "Kenichiro Yoshida",
                Email = "dohung.csharp@gmail.com",
                Bank = "Sumitomo Mitsui Banking Corporation",
                AccountNumber = "5566778899",
                BankAddress = "Tầng 6 - Toà Nhà 33T6 - Đường Lý Thường Kiệt - Hoàn Kiếm - Hà Nội",
                AddressOne = "15 Đường 3/2, Quận 10, TP. Hồ Chí Minh",
                AddressTwo = "67 Hàng Bài, Quận Hoàn Kiếm, Hà Nội",
                Phone = "0967890123",
                Fax = "833005",
                NationCode = "0084",
                ProvinceCode = "06",
                DistrictCode = "006"
            },

            new Supplier()
            {
                Name = "Asus",
                Alias = "asus",
                Description = "Nhà cung cấp thiết bị điện tử và máy tính Asus",
                Delegate = "Samson Hu",
                Email = "dohung.csharp@gmail.com",
                Bank = "Taiwan Cooperative Bank",
                AccountNumber = "9988776655",
                BankAddress = "Tầng 7 - Toà Nhà 37T7 - Đường Xuân Thủy - Cầu Giấy - Hà Nội",
                AddressOne = "23 Thái Hà, Đống Đa, Hà Nội",
                AddressTwo = "88 Cao Thắng, Quận 3, TP. Hồ Chí Minh",
                Phone = "0987654323",
                Fax = "833006",
                NationCode = "0084",
                ProvinceCode = "07",
                DistrictCode = "007"
            },

            new Supplier()
            {
                Name = "Dell",
                Alias = "dell",
                Description = "Nhà cung cấp máy tính và thiết bị điện tử Dell",
                Delegate = "Michael Dell",
                Email = "dohung.csharp@gmail.com",
                Bank = "Citibank",
                AccountNumber = "2233445566",
                BankAddress = "Tầng 8 - Toà Nhà 40T8 - Đường Nguyễn Chí Thanh - Đống Đa - Hà Nội",
                AddressOne = "49 Lê Ngọc Hân, Hai Bà Trưng, Hà Nội",
                AddressTwo = "34 Lê Duẩn, Quận 1, TP. Hồ Chí Minh",
                Phone = "0976543210",
                Fax = "833007",
                NationCode = "0084",
                ProvinceCode = "08",
                DistrictCode = "008"
            },

            new Supplier()
            {
                Name = "Lenovo",
                Alias = "lenovo",
                Description = "Nhà cung cấp máy tính và thiết bị điện tử Lenovo",
                Delegate = "Yang Yuanqing",
                Email = "dohung.csharp@gmail.com",
                Bank = "China Construction Bank",
                AccountNumber = "6655778899",
                BankAddress = "Tầng 9 - Toà Nhà 45T9 - Đường Trần Phú - Ba Đình - Hà Nội",
                AddressOne = "5 Đinh Tiên Hoàng, Quận Hoàn Kiếm, Hà Nội",
                AddressTwo = "60 Nguyễn Huệ, Quận 1, TP. Hồ Chí Minh",
                Phone = "0912345679",
                Fax = "833008",
                NationCode = "0084",
                ProvinceCode = "09",
                DistrictCode = "009"
            },

            new Supplier()
            {
                Name = "HP",
                Alias = "hp",
                Description = "Nhà cung cấp máy tính và thiết bị điện tử HP",
                Delegate = "Enrique Lores",
                Email = "dohung.csharp@gmail.com",
                Bank = "Wells Fargo",
                AccountNumber = "1233211234",
                BankAddress = "Tầng 10 - Toà Nhà 50T10 - Đường Trần Hưng Đạo - Hoàn Kiếm - Hà Nội",
                AddressOne = "15 Lý Thái Tổ, Quận Hoàn Kiếm, Hà Nội",
                AddressTwo = "75 Nam Kỳ Khởi Nghĩa, Quận 1, TP. Hồ Chí Minh",
                Phone = "0932109876",
                Fax = "833009",
                NationCode = "0084",
                ProvinceCode = "10",
                DistrictCode = "010"
            },
            new Supplier()
            {
                Name = "Acer",
                Alias = "acer",
                Description = "Nhà cung cấp thiết bị điện tử và máy tính Acer",
                Delegate = "Jason Chen",
                Email = "dohung.csharp@gmail.com",
                Bank = "ANZ Bank",
                AccountNumber = "3344556677",
                BankAddress = "Tầng 11 - Toà Nhà 55T11 - Đường Lê Lợi - Hoàn Kiếm - Hà Nội",
                AddressOne = "22 Thanh Niên, Tây Hồ, Hà Nội",
                AddressTwo = "18 Phạm Ngọc Thạch, Quận 3, TP. Hồ Chí Minh",
                Phone = "0943210987",
                Fax = "833010",
                NationCode = "0084",
                ProvinceCode = "11",
                DistrictCode = "011"
            },

            new Supplier()
            {
                Name = "Toshiba",
                Alias = "toshiba",
                Description = "Nhà cung cấp thiết bị điện tử và máy tính Toshiba",
                Delegate = "Nobuaki Kurumatani",
                Email = "dohung.csharp@gmail.com",
                Bank = "Mizuho Bank",
                AccountNumber = "5566887799",
                BankAddress = "Tầng 12 - Toà Nhà 60T12 - Đường Điện Biên Phủ - Ba Đình - Hà Nội",
                AddressOne = "89 Cầu Giấy, Quận Cầu Giấy, Hà Nội",
                AddressTwo = "33 Lê Duẩn, Quận 1, TP. Hồ Chí Minh",
                Phone = "0987654324",
                Fax = "833011",
                NationCode = "0084",
                ProvinceCode = "12",
                DistrictCode = "012"
            },

            new Supplier()
            {
                Name = "Panasonic",
                Alias = "panasonic",
                Description = "Nhà cung cấp thiết bị điện tử Panasonic",
                Delegate = "Kazuhiro Tsuga",
                Email = "dohung.csharp@gmail.com",
                Bank = "Bank of Tokyo-Mitsubishi UFJ",
                AccountNumber = "6677889900",
                BankAddress = "Tầng 13 - Toà Nhà 65T13 - Đường Trần Nhân Tông - Hai Bà Trưng - Hà Nội",
                AddressOne = "50 Lý Quốc Sư, Hoàn Kiếm, Hà Nội",
                AddressTwo = "27 Lê Quý Đôn, Quận 3, TP. Hồ Chí Minh",
                Phone = "0976543214",
                Fax = "833012",
                NationCode = "0084",
                ProvinceCode = "13",
                DistrictCode = "013"
            },
            new Supplier()
            {
                Name = "Google",
                Alias = "google",
                Description = "Nhà cung cấp dịch vụ công nghệ thông tin và sản phẩm kỹ thuật số",
                Delegate = "Sundar Pichai",
                Email = "dohung.csharp@gmail.com",
                Bank = "Silicon Valley Bank",
                AccountNumber = "2244668800",
                BankAddress = "Tầng 14 - Toà Nhà 14 Silicon - Đường Phạm Hùng - Nam Từ Liêm - Hà Nội",
                AddressOne = "Khu Công nghệ cao, Quận 9, TP. Hồ Chí Minh",
                AddressTwo = "52 Lý Thường Kiệt, Hoàn Kiếm, Hà Nội",
                Phone = "0912233445",
                Fax = "833014",
                NationCode = "0084",
                ProvinceCode = "14",
                DistrictCode = "014"
            },

            new Supplier()
            {
                Name = "Facebook",
                Alias = "facebook",
                Description = "Nhà cung cấp dịch vụ mạng xã hội và quảng cáo trực tuyến",
                Delegate = "Mark Zuckerberg",
                Email = "dohung.csharp@gmail.com",
                Bank = "Bank of America",
                AccountNumber = "5566778899",
                BankAddress = "Tầng 15 - Toà Nhà 15A - Đường Lê Hồng Phong - Ba Đình - Hà Nội",
                AddressOne = "38 Yên Phụ, Tây Hồ, Hà Nội",
                AddressTwo = "90 Nguyễn Huệ, Quận 1, TP. Hồ Chí Minh",
                Phone = "0987654321",
                Fax = "833015",
                NationCode = "0084",
                ProvinceCode = "15",
                DistrictCode = "015"
            },
            new Supplier()
            {
                Name = "Google",
                Alias = "google",
                Description = "Nhà cung cấp thiết bị điện tử và dịch vụ công nghệ thông tin Google",
                Delegate = "Sundar Pichai",
                Email = "dohung.csharp@gmail.com",
                Bank = "Bank of America",
                AccountNumber = "7788990011",
                BankAddress = "Tầng 14 - Toà Nhà 70T14 - Đường Bà Triệu - Hai Bà Trưng - Hà Nội",
                AddressOne = "35 Nguyễn Chí Thanh, Đống Đa, Hà Nội",
                AddressTwo = "48 Thảo Điền, Quận 2, TP. Hồ Chí Minh",
                Phone = "0912345671",
                Fax = "833013",
                NationCode = "0084",
                ProvinceCode = "14",
                DistrictCode = "014"
            },

            new Supplier()
            {
                Name = "Microsoft",
                Alias = "microsoft",
                Description = "Nhà cung cấp phần mềm và dịch vụ công nghệ thông tin Microsoft",
                Delegate = "Satya Nadella",
                Email = "dohung.csharp@gmail.com",
                Bank = "JPMorgan Chase Bank",
                AccountNumber = "1122334455",
                BankAddress = "Tầng 15 - Toà Nhà 75T15 - Đường Nguyễn Du - Hoàn Kiếm - Hà Nội",
                AddressOne = "18B Phố Huế, Hai Bà Trưng, Hà Nội",
                AddressTwo = "22 Lê Lai, Quận 1, TP. Hồ Chí Minh",
                Phone = "0987654325",
                Fax = "833014",
                NationCode = "0084",
                ProvinceCode = "15",
                DistrictCode = "015"
            },

            new Supplier()
            {
                Name = "IBM",
                Alias = "ibm",
                Description = "Nhà cung cấp phần mềm và dịch vụ công nghệ thông tin IBM",
                Delegate = "Arvind Krishna",
                Email = "dohung.csharp@gmail.com",
                Bank = "Citi Bank",
                AccountNumber = "5566778899",
                BankAddress = "Tầng 16 - Toà Nhà 80T16 - Đường Lê Thanh Nghị - Hai Bà Trưng - Hà Nội",
                AddressOne = "50A Đường Võ Văn Tần, Quận 3, TP. Hồ Chí Minh",
                AddressTwo = "30 Lê Thánh Tôn, Quận 1, TP. Hồ Chí Minh",
                Phone = "0943210988",
                Fax = "833015",
                NationCode = "0084",
                ProvinceCode = "16",
                DistrictCode = "016"
            },

            new Supplier()
            {
                Name = "Oracle",
                Alias = "oracle",
                Description = "Nhà cung cấp phần mềm và dịch vụ công nghệ thông tin Oracle",
                Delegate = "Safra Catz",
                Email = "dohung.csharp@gmail.com",
                Bank = "Wells Fargo",
                AccountNumber = "7788990012",
                BankAddress = "Tầng 17 - Toà Nhà 85T17 - Đường Trần Hưng Đạo - Hoàn Kiếm - Hà Nội",
                AddressOne = "15A Lý Thái Tổ, Quận Hoàn Kiếm, Hà Nội",
                AddressTwo = "44 Lê Lai, Quận 1, TP. Hồ Chí Minh",
                Phone = "0912345672",
                Fax = "833016",
                NationCode = "0084",
                ProvinceCode = "17",
                DistrictCode = "017"
            },

            new Supplier()
            {
                Name = "Cisco",
                Alias = "cisco",
                Description = "Nhà cung cấp thiết bị mạng Cisco",
                Delegate = "Chuck Robbins",
                Email = "dohung.csharp@gmail.com",
                Bank = "Bank of China",
                AccountNumber = "9988776656",
                BankAddress = "Tầng 18 - Toà Nhà 90T18 - Đường Lê Ngọc Hân - Hoàn Kiếm - Hà Nội",
                AddressOne = "10B Đường Điện Biên Phủ, Ba Đình, Hà Nội",
                AddressTwo = "25 Lê Thánh Tôn, Quận 1, TP. Hồ Chí Minh",
                Phone = "0987654326",
                Fax = "833017",
                NationCode = "0084",
                ProvinceCode = "18",
                DistrictCode = "018"
            },

            new Supplier()
            {
                Name = "Adobe",
                Alias = "adobe",
                Description = "Nhà cung cấp phần mềm đồ họa Adobe",
                Delegate = "Shantanu Narayen",
                Email = "dohung.csharp@gmail.com",
                Bank = "Standard Chartered Bank",
                AccountNumber = "1122334456",
                BankAddress = "Tầng 19 - Toà Nhà 95T19 - Đường Lê Đại Hành - Hai Bà Trưng - Hà Nội",
                AddressOne = "25 Lê Duẩn, Quận 1, TP. Hồ Chí Minh",
                AddressTwo = "40 Hàng Bài, Quận Hoàn Kiếm, Hà Nội",
                Phone = "0912345673",
                Fax = "833018",
                NationCode = "0084",
                ProvinceCode = "19",
                DistrictCode = "019"
            },

            new Supplier()
            {
                Name = "Nokia",
                Alias = "nokia",
                Description = "Nhà cung cấp thiết bị di động và mạng Nokia",
                Delegate = "Pekka Lundmark",
                Email = "dohung.csharp@gmail.com",
                Bank = "Deutsche Bank",
                AccountNumber = "5566778890",
                BankAddress = "Tầng 20 - Toà Nhà 100T20 - Đường Hàm Nghi - Nam Từ Liêm - Hà Nội",
                AddressOne = "68A Nguyễn Huệ, Quận 1, TP. Hồ Chí Minh",
                AddressTwo = "10 Hàng Bài, Quận Hoàn Kiếm, Hà Nội",
                Phone = "0943210989",
                Fax = "833019",
                NationCode = "0084",
                ProvinceCode = "20",
                DistrictCode = "020"
            },
        };

        await _context.Suppliers.AddRangeAsync(suppliers);
        await _context.SaveChangesAsync();
    }
}