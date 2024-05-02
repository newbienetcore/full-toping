## .IsRequired
```
+ không sử dụng IsRequired thì mặc định nó sẽ là .IsRequired, nhưng nếu ở entity có để ? => mặc định IsRequired(false)
=> nếu yêu cầu không bắt buộc thì dùng IsRequired(false) or ? (public int? Code {get;set;}) là được
=> còn đâu muốn là Required thì để mặc định không cần config gì nha
```

## IsUnicode
```
+ Mặc định nếu không dùng gì sẽ là InUnicode rồi
=> Nếu muốn nó không phải Unicode thì mới cần config là IsUnicode(false)
```

# Enum

```
public enum DiscountUnit
{
    Percentage = 0,  // Phần trăm
    Amount = 1,      // Số tiền cụ thể
    Other = 2        // Các đơn vị khác (bạn có thể mở rộng theo nhu cầu)
}

builder.Property(cd => cd.DiscountUnit)
    .HasConversion(
        v => v.ToString(),      // Chuyển đổi từ enum sang chuỗi để lưu trong cơ sở dữ liệu
        v => (DiscountUnit)Enum.Parse(typeof(DiscountUnit), v)  // Chuyển đổi từ chuỗi sang enum khi đọc từ cơ sở dữ liệu
    );

```
