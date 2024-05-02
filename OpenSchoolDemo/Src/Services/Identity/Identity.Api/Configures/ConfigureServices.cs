using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Http.Features;

namespace Identity.Api.Configures;

public static class ConfigureServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(_ => configuration);
        
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        
        // Điều này đăng ký dịch vụ xác thực chứng chỉ vào hệ thống xác thực của ứng dụng
        services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();
        
        // Đoạn mã này đang cấu hình các tùy chọn cho xử lý dữ liệu form.
        // Cụ thể, nó đang cấu hình giới hạn độ dài giá trị, độ dài của phần thân và phần đầu của yêu cầu dữ liệu đa phần (multipart)
        services.Configure<FormOptions>(x =>
        {
            x.ValueLengthLimit = int.MaxValue;
            x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            x.MultipartHeadersLengthLimit = int.MaxValue;
        });

        return services;
    }
}